using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMP4109_Challenge1
{
  public class Decoder
  {
    /// <summary>
    /// Defines the start of possible key sizes to count up from
    /// </summary>
    public static readonly int STARTING_KEY_SIZE = 2;

    /// <summary>
    /// Offset of ASCII character 'A' in decimal
    /// </summary>
    public static readonly int ALPHABET_DECIMAL_OFFSET = 65;

    /// <summary>
    /// Offset of Alphabet 0 = A, 25 = Z
    /// </summary>
    public static readonly int ALPHABET_CHARACTER_OFFSET = 25;

    /// <summary>
    /// Given a cipherText, break the text as a Vigenere Cipher
    /// </summary>
    /// <param name="cipherText">The cipher to break</param>
    public void BreakVigenere(string cipherText)
    {
      // Get a dictionary of trigraphs
      Dictionary<string, int> dictionary = this.GetGraphDictionary(cipherText, 3);

      // If we could not find any trigraphs, attempt to solve on digraphs
      if(dictionary.Count <= 0)
        dictionary = this.GetGraphDictionary(cipherText, 2);

      // If we STILL cannot find any digraphs
      if (dictionary.Count <= 0)
      {
        // GIVE UP.
        Console.WriteLine("Could not find any graphs. This cipher is one-time padded. Exiting. . .");
        return;
      }

      Dictionary<int, int> keysizeDictionary = new Dictionary<int, int>();
      foreach (KeyValuePair<string, int> kvp in dictionary)
      {
        // Begin calculating distances between graphs
        int distance = this.GetDistance(cipherText, kvp.Key);

        // Determine possible key size via distance factors
        List<int> keySizeFactors = this.GetKeyFactors(distance);
        foreach (int possibleKeySize in this.GetKeyFactors(distance))
        {
          if (!keysizeDictionary.ContainsKey(possibleKeySize))
            keysizeDictionary.Add(possibleKeySize, 0);

          // Count the occurrences of a key of this size
          keysizeDictionary[possibleKeySize]++;
        }
      }

      // Order the dictionary descending
      // Likely Key size is the one with the most common factors
      List<KeyValuePair<int, int>> listKeySize = keysizeDictionary.ToList();
      listKeySize.Sort((kvp1, kvp2) => kvp2.Value.CompareTo(kvp1.Value));

      Console.WriteLine("Key Size is: " + listKeySize[0].Key + ", Frequency of Size: " + listKeySize[0].Value);
      this.BreakVigenere(listKeySize[0].Key, cipherText);
    }

    /// <summary>
    /// Given the size of a key and the ciphertext, break the cipher
    /// as a Vigenere Cipher
    /// </summary>
    /// <param name="keySize">Size of the key</param>
    /// <param name="cipherText">The ciphertext to break</param>
    private PlaintextCipherPair BreakVigenere(int keySize, string cipherText)
    {
      PlaintextCipherPair data = new PlaintextCipherPair();
      data.ciphertext = cipherText;

      // Define a two dimensional list to partition the letters
      // Encrypted by a particular key (character)
      List<List<char>> letterPartitions = new List<List<char>>();
      for (int i = 0; i < keySize; i++)
        letterPartitions.Add(new List<char>());

      // Divide the text into the lists
      for (int i = 0; i < cipherText.Length; i++)
        letterPartitions[i % keySize].Add(cipherText[i]);

      List<List<char>> allDecryptedCharacters = new List<List<char>>();

      StringBuilder key = new StringBuilder();
      char partialKey;
      foreach (List<char> partition in letterPartitions)
      {
        // Break the divided letters as a regular Caeser Cipher
        allDecryptedCharacters.Add(this.BreakCaeser(partition, out partialKey));
        Console.WriteLine("Partial Key: " + partialKey);
        key.Append(partialKey);
      }

      data.key = key.ToString();
      Console.WriteLine("Found Key: " + key.ToString());

      StringBuilder plaintext = new StringBuilder();
      for(int i = 0; i < cipherText.Length; i++)
      {
        int listIndex = i % keySize;
        plaintext.Append(allDecryptedCharacters[listIndex][i/keySize]);
      }

      data.plaintext = plaintext.ToString();
      Console.WriteLine("Plaintext: " + data.plaintext);

      return data;
    }

    /// <summary>
    /// Break a Caeser Cipher
    /// </summary>
    /// <param name="caeserCipherText">The characters to break</param>
    /// <param name="partialKey">out parameter to return a partial key</param>
    /// <returns>A list containing the decoded text</returns>
    public List<char> BreakCaeser(List<char> caeserCipherText, out char partialKey)
    {
      // Initialize values
      partialKey = '!';
      double bestChiScore = double.MaxValue;
      List<char> bestPlaintext = new List<char>();
      List<char> decryptedChars = new List<char>();

      // Loop from Z to A.
      for (int i = -ALPHABET_CHARACTER_OFFSET; i <= 0; i++)
      {
        // Empty the list
        decryptedChars = new List<char>();

        foreach (char c in caeserCipherText)
        {
          // Shift the character 'i' times
          int decryptedChar = c + i;

          // If our character is out of range, roll over the alphabet
          if (decryptedChar < 'A')
            decryptedChar = decryptedChar + 90;

          if (decryptedChar > 'Z')
            decryptedChar = decryptedChar - 65 - (26 + i);

          decryptedChars.Add(Convert.ToChar(decryptedChar));
        }

        // Calculate the Chi-Square Score to determine if it is English
        double newChiScore = this.CalculateChiSquared(decryptedChars);
        if (newChiScore < bestChiScore)
        {
          bestPlaintext = decryptedChars;
          bestChiScore = newChiScore;
          partialKey = Convert.ToChar(ALPHABET_DECIMAL_OFFSET + i * -1); // Invert the value of i so it is non-negative
        }
      }

      return bestPlaintext;
    }

    /// <summary>
    /// Calculate the value of Chi-Squared.
    /// Determines how well a distribution fits another distribution.
    /// If score == 0 then perfect fit
    /// </summary>
    /// <param name="characters">The list of characters to parse</param>
    /// <returns>A double containing the score</returns>
    private double CalculateChiSquared(List<char> characters)
    {
      int numCharacters = characters.Count;
      double chiSquare = 0;

      // Compare all letters to their expected frequency
      for (int i = 'A'; i <= 'Z'; i++)
      {
        int numFoundChars = characters.Where(c => c == i).Count();
        double expectedFrequency = Frequency.FrequencyDictionary[Convert.ToChar(i)] * characters.Count();
        double characterRatio = Math.Pow(numFoundChars - expectedFrequency, 2) / expectedFrequency;

        chiSquare += characterRatio;
      }

      return chiSquare;
    }

    /// <summary>
    /// Get possible factors for a given key
    /// </summary>
    /// <param name="possibleKeySize"></param>
    /// <returns></returns>
    private List<int> GetKeyFactors(int possibleKeySize)
    {
      List<int> keyFactors = new List<int>();
      for (int i = STARTING_KEY_SIZE; i <= possibleKeySize; i++)
      {
        if (possibleKeySize % i == 0)
          keyFactors.Add(i);
      }

      return keyFactors;
    }

    /// <summary>
    /// For a ciphertext and a given graph,
    /// Determine the distance between the two
    /// </summary>
    /// <param name="cipherText">The ciphertext to analyze</param>
    /// <param name="graph">The graph/gram to search for</param>
    /// <returns></returns>
    private int GetDistance(string cipherText, string graph)
    {
      int indexOfFirst = cipherText.IndexOf(graph);
      int indexOfSecond = cipherText.IndexOf(graph, indexOfFirst + graph.Length);

      return indexOfSecond - indexOfFirst;
    }

    /// <summary>
    /// Parse a ciphertext to determine if there are any duplicate graphs/grams present
    /// </summary>
    /// <param name="ciphertext">The ciphertext to parse</param>
    /// <param name="graphSize">The size of the graph</param>
    /// <returns></returns>
    private Dictionary<string, int> GetGraphDictionary(string ciphertext, int graphSize)
    {
      Dictionary<string, int> graphDictionary = new Dictionary<string, int>();
      // Iterate and search for graphs
      // NOTE: ciphertext.Length - graphSize + 1 ensures our index always stops
      //       at the last graph. + 1 because our size is a 0-based index.
      for (int i = 0; i < ciphertext.Length - graphSize + 1; i++)
      {
        string graph = ciphertext.Substring(i, graphSize);
        if (!graphDictionary.ContainsKey(graph))
          graphDictionary.Add(graph, 0);

        graphDictionary[graph]++;
      }

      // Filter the dictionary to only include graphs that had duplicate
      graphDictionary = graphDictionary
                          .Where(kvp => kvp.Value > 1)
                          .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

      return graphDictionary;
    }
  }
}
