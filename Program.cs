using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMP4109_Challenge1
{
  class Program
  {
    /// <summary>
    /// Character Frequency graph for English (includes space as a char)
    /// Taken from http://www.data-compression.com/english.html
    /// </summary>
    public static Dictionary<char, double> FrequencyDictionarySpace = new Dictionary<char, double>()
      {
        { 'E', 0.1041442 },
        { 'T', 0.0729357 },
        { 'A', 0.0651738 },
        { 'O', 0.0596302 },
        { 'I', 0.0558094 },
        { 'N', 0.0564513 },
        { 'S', 0.0515760 },
        { 'H', 0.0492888 },
        { 'R', 0.0497563 },
        { 'D', 0.0349835 },
        { 'L', 0.0331490 },
        { 'C', 0.0217339 },
        { 'U', 0.0225134 },
        { 'M', 0.0202124 },
        { 'F', 0.0197881 },
        { 'W', 0.0171272 },
        { 'G', 0.0158610 },
        { 'P', 0.0137645 },
        { 'Y', 0.0145984 },
        { 'B', 0.0124248 },
        { 'V', 0.0082903 },
        { 'K', 0.0050529 },
        { 'X', 0.0013692 },
        { 'J', 0.0009033 },
        { 'Q', 0.0008606 },
        { 'Z', 0.0007836 },
        { ' ', 0.1918182 }
    };

    /// <summary>
    /// Character Frequency graph for English
    /// Taken from https://en.wikipedia.org/wiki/Letter_frequency
    /// </summary>
    public static Dictionary<char, double> FrequencyDictionary = new Dictionary<char, double>()
    {
      { 'E', 0.12702 },
      { 'T', 0.09056 },
      { 'A', 0.08167 },
      { 'O', 0.07507 },
      { 'I', 0.06966 },
      { 'N', 0.06749 },
      { 'S', 0.06327 },
      { 'H', 0.06094 },
      { 'R', 0.05987 },
      { 'D', 0.04253 },
      { 'L', 0.04025 },
      { 'C', 0.02782 },
      { 'U', 0.02758 },
      { 'M', 0.02406 },
      { 'F', 0.02228 },
      { 'W', 0.02360 },
      { 'G', 0.02015 },
      { 'P', 0.01929 },
      { 'Y', 0.01974 },
      { 'B', 0.01492 },
      { 'V', 0.00978 },
      { 'K', 0.00772 },
      { 'X', 0.00150 },
      { 'J', 0.00153 },
      { 'Q', 0.00095 },
      { 'Z', 0.00074 },
    };

    /// <summary>
    /// Defines the start of possible key sizes to count up from
    /// </summary>
    public static readonly int STARTING_KEY_SIZE = 2;

    public static string firstCipher = "YPWMRREGLVXTRXSIXZARYSSYISYXLPJECQIGIVDIIDLICEKLMRLRHDLITWRPZICQIYXMZRIOEKLMRHMXSXLPPEYHRZAJCSDPRWZPMOMXTWMXTSDWMMPIESHZERJJECQM";
    public static string secondCipher = "ZPQFVODQCAKQFQMKLEZVHZMTVCIHPMTMDRGTWFQZUUUEAIIDCQKZNFBLZAXBNWEPIXWGYLOBUEIHAACJYIXWCTKAYAIQAFASMZEIRNDPYAMZNQKARCWSBTTAHZMTVCIHPMTMDRGZWZZCXQESQZAQWNCQFSQZBTPVKFOCMZMETVZWFSMSQZOWLQFDKGZDTMXIFPTKXMEPOKYLBXQJQWXUQOJEKAXJOVUYOZPQNWTAOTWAAFSWAOTENXMCFMTKUPACQFSVKZHPAOOZLTYXUNSKLGANXWYEPKABPMIPOPVZZQDWLBTPJXIUYENQOSPGAEFXVTUPLZPQXBNMBCIIBUNIRCBDPUBAQIRTFSQYQEEPGBUQGUCEEQISMMIHMXQQYPUYGUCDPIXGAFKGVUYAZIZETECZOMXAFLVJIZJBNQZRAGQPEWEWGTVGVKQWXUAQTGVSFIMMFSMYXQPKNXMEBKZZDGUCMNBAIXWGNMMCLKKAOMZPQMZGQZHIBMYLBXQJHPOKTSIYJQPVLMPTVZWKZCXUUYLHGKZCXJMMMRNUDPTWITBOAEFKNINTHGZDPTEQYAZUJMMTKKATVIQPPVIMFSIZIZJBNQZRAUUUYLHWSRQTOXJCYMRFTIWGWLNIHPMBWXGMJXGCMRGNJKNIZNMZPMEAUUQEPOVWPZYPMGMIPADMTBADMKQFLAZPQQQTIXLVJKXTVIPUYOVZAZNUNFSMTWZPFOAFPVIMAQOULFSMGZSFUKVFRWKAEZUKBTTVMTUVMZPUDQXMRFAKBAAZUDQEPGBUPFOAFDIEASZLLWDAZUWROMTQQDNGQFSITLITBNWGENGQFSQGUZZBNQZRJABELGYUMYBNMNLJKTRTANQELLKIPRQBMMHIEQEYBOBUEKUCXOVUBTLDKMHZTBMPMGIPMYKKQFAZUDQDGUCQIQYBMYLYWFSMXMRZZKJKJWAZAHVGZSFUKVFDGUCPZVZYQOWNLQLZYIKDOULUSIJVFEPUCSSBUNFSIZIZOXXWYABRGHLVOATP";
    public static string thirdCipher = "VGHOXTGPUACJOOOACPZKAWJJRRVNHXJAADINMJDJEXRZSHEXOCLEVTRIUWQJNJMISSOCZWRIYVFOESYVNNIFONAKVJJAWAVXJIHHR";

    static void Main(string[] args)
    {
      Program p = new Program();
      Dictionary<string, int> dictionary = p.GetGraphDictionary(firstCipher, 3);
      Dictionary<int, int> keysizeDictionary = new Dictionary<int, int>();
      foreach (KeyValuePair<string, int> kvp in dictionary)
      {
        int distance = p.GetDistance(firstCipher, kvp.Key);
        List<int> keySizeFactors = p.GetKeyFactors(distance);
        foreach (int possibleKeySize in p.GetKeyFactors(distance))
        {
          if (!keysizeDictionary.ContainsKey(possibleKeySize))
            keysizeDictionary.Add(possibleKeySize, 0);

          keysizeDictionary[possibleKeySize]++;
        }
      }

      // Order the dictionary descending
      // Likely Key size is the one with the most common factors
      List<KeyValuePair<int, int>> listKeySize = keysizeDictionary.ToList();
      listKeySize.Sort((kvp1, kvp2) => kvp2.Value.CompareTo(kvp1.Value));

      Console.WriteLine("Key Size is: " + listKeySize[0].Key + ", Frequency: " + listKeySize[0].Value);
      p.BreakVigenere(listKeySize[0].Key, firstCipher);

      Console.ReadKey();
    }

    public void BreakVigenere(int keySize, string cipherText)
    {
      // Define a two dimensional list to partition the letters
      // Encrypted by a particular key (character)
      List<List<char>> letterPartitions = new List<List<char>>();
      for (int i = 0; i < keySize; i++)
        letterPartitions.Add(new List<char>());

      // Divide the text into the lists
      for (int i = 0; i < cipherText.Length; i++)
        letterPartitions[i % keySize].Add(cipherText[i]);

      StringBuilder key = new StringBuilder();
      foreach (List<char> partition in letterPartitions)
      {
        char partialKey = '!';
        List<char> decryptedChars = this.BreakCaeser(partition, out partialKey);
        Console.WriteLine("Partial Key: " + partialKey);
        key.Append(partialKey);
      }

      Console.WriteLine("Found Key: " + key.ToString());
    }


    public List<char> BreakCaeser(List<char> characters, out char partialKey)
    {
      partialKey = '!';
      double bestChiScore = double.MaxValue;
      List<char> decryptedChars = new List<char>();

      for (int i = -25; i <= 0; i++)
      {
        decryptedChars = new List<char>();

        foreach (char c in characters)
        {
          int decryptedChar = c + i;
          if (decryptedChar < 'A')
            decryptedChar = decryptedChar + 26; // Roll over

          decryptedChars.Add(Convert.ToChar(decryptedChar));
        }

        double newChiScore = this.CalculateChiSquared(decryptedChars);
        if(newChiScore < bestChiScore)
        {
          bestChiScore = newChiScore;
          partialKey = Convert.ToChar(65 + i * -1);
        }
      }

      return decryptedChars;
    }

    private double CalculateChiSquared(List<char> characters)
    {
      int numCharacters = characters.Count;
      double chiSquare = 0;

      for (int i = 'A'; i <= 'Z'; i++)
      {
        int numFoundChars = characters.Where(c => c == i).Count();
        double expectedFrequency = FrequencyDictionary[Convert.ToChar(i)] * characters.Count();
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
    public List<int> GetKeyFactors(int possibleKeySize)
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
    /// <param name="cipherText"></param>
    /// <param name="graph"></param>
    /// <returns></returns>
    public int GetDistance(string cipherText, string graph)
    {
      int indexOfFirst = cipherText.IndexOf(graph);
      int indexOfSecond = cipherText.IndexOf(graph, indexOfFirst + graph.Length);

      return indexOfSecond - indexOfFirst;
    }

    public Dictionary<string, int> GetGraphDictionary(string ciphertext, int graphSize)
    {
      Dictionary<string, int> graphDictionary = new Dictionary<string, int>();
      // Iterate and search for graphs
      // NOTE: ciphertext.Length - graphSize + 1 ensures our index always stops
      //       at the last graph. +1 because our size is a 0-based index.
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
