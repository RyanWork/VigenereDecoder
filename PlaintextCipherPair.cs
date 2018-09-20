using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP4109_Challenge1
{
  /// <summary>
  /// Data Class for a plaintext cipher pair with a key
  /// </summary>
  public class PlaintextCipherPair
  {
    /// <summary>
    /// The key used to encrypt/decrypt
    /// </summary>
    public string key { get; set; }
    
    /// <summary>
    /// The ciphertext
    /// </summary>
    public string ciphertext { get; set; }

    /// <summary>
    /// The plaintext
    /// </summary>
    public string plaintext { get; set; }

    public PlaintextCipherPair()
    {
      this.key = string.Empty;
      this.ciphertext = string.Empty;
      this.plaintext = string.Empty;
    }

    public PlaintextCipherPair(string key, string ciphertext, string plaintext)
    {
      this.key = key;
      this.ciphertext = ciphertext;
      this.plaintext = plaintext;
    }
  }
}
