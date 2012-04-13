/* ===========================
File : ecb.cs

Credit:
- Coded by (aka in cyber) Vsio Stitched
- Problem case inspired from my past in high school

Misc:
- Written in C# programming laguage

License:
- Free to use
- May include me or not in credit if included in other project
=========================== */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


class ECB {

    // FIELD

    private String inputText_; /* text input whicih will be encrypted or decrypted */
    private String key_; /* binary input */
    private String outputText_; /* text output which is resulted from encryption or decryption */
	private byte[] outputBytes_; /* output in bytes */

	
    // CONSTRUCTOR

    public ECB()
    /* constructs ECB instance */
    {
        this.inputText_ = "";
        this.key_ = "";
        this.outputText_ = "";
    }

    public ECB(String _newInputText, String _newKey, String _newOutputText)
    /* constructs ECB instance */
    {
        this.inputText_ = _newInputText;
        this.key_ = _newKey;
        this.outputText_ = _newOutputText;
    }


    // ACCESSOR GET

    public String getInputText()
    /* gets inputText_ value */
    {
        return this.inputText_;
    }

    public String getKey()
    /* returns key_ value */
    {
        return this.key_;
    }

    public String getOutputText()
    /* returns outputText_ value */
    {
        return this.outputText_;
    }

	public byte[] getOutputBytes()
	/* returns outputBytes_ value */
	{
		return this.outputBytes_;
	}
	
    // ACCESSOR SET

    public void setInputText(String _newInputText)
    /* changes inputText_ value */
    {
        this.inputText_ = _newInputText;
    }

    public void setKey(String _newKey)
    /* changes key_ value */
    {
        this.key_ = _newKey;
    }

    public void setOutputText(String _newOutputText)
    /* changes outputText_ value */
    {
        this.outputText_ = _newOutputText;
    }

	public void setOutputBytes(byte[] _newOutputBytes)
	/* returns outputBytes_ value */
	{
		this.outputBytes_ = _newOutputBytes;
	}
	
	
    // PREDICATE

	
    // METHOD

    public void menu()
    /* displays command menu */
    {
		String index = "0";
                
        while (index[0] != '3') {
            Console.WriteLine("\n\n==> MENU\n\n");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect a number : ");
            index = Console.ReadLine();
            Console.WriteLine();
            switch(index[0]) {
                case '1': {
                    loadFile();
                    inputKey();
                    encipher();
                    saveFile(0);
					goto case '3';
				}
                case '2': {
                    loadFile();
                    inputKey();
                    decipher();
                    saveFile(1);
					goto case '3';
                }
                case '3': {
                    index = "3";
					break;
                }
                default:{
                    Console.WriteLine("> Invalid Input");
                    index = "0";
					break;
                }

            }
        }
    }
	
	public void inputKey()
	/* input key_ value */
	{
		String value = " ";
		Regex regexConstraint = new Regex("^[0-1]+$");
		
		while (!regexConstraint.IsMatch(value)) {
			Console.Write("\nInput binary string: ");
			value = Console.ReadLine();
			if (!regexConstraint.IsMatch(value)) {
				Console.WriteLine("> Error: Only 0 or 1 is allowed");				
			}
		}
		
		this.key_ = value;
	}
	
	public String encryptionBlock(String _plainText)
	/* encryption blocks (in this example, a simple xor encryption is used) (You can define your own Encryption Block here) */
	{
		String plainText = _plainText;
		String output = "";
		String key = this.key_;
		
		for (int i=0;i!=key.Length;i++) {
			output += xorBit(plainText[i],key[i]);
		}
		
		return output;
	}

	public String decryptionBlock(String _cipherText)
	/* decryption blocks (in this example, a simple xor decryption is used) (You can define your own Decryption Block here) */
	{
		String cipherText = _cipherText;
		String output = "";
		String key = this.key_;
		
		for (int i=0;i!=key.Length;i++) {
			output += xorBit(cipherText[i],key[i]);
		}
		
		return output;
	}
	
	public byte[] binaryStringToBytes(string _binaryString) 
	/* converts binary string to binary bytes */
	{
		string binary = _binaryString;
		int i = 0;
		int j = 0;
		BitArray bits = new BitArray(8);
		byte[] bytes = new byte[binary.Length/8];
		
		while (j!=binary.Length) {		
			for (i=0;i!=8;i++) {
				
				bits.Set(i,(binary[j+i]=='1'?true:false));
			}
			
			bits.CopyTo(bytes,j/8);
			j+=8;
		}
		
		return bytes;
	}
	
	public char binaryToChar(string _binaryString) 
	/* converts binary to char */
	{
		UTF8Encoding utf8 = new UTF8Encoding();
		string binary = _binaryString;
		byte[] bytes = binaryStringToBytes(binary);
		string output = utf8.GetString(bytes);
		
		return output[0];
	}
	
	public string binaryToString(string _binaryString)
	/* converts binary to string */
	{
		string binaries = _binaryString;
		string output = "";
		int i = 0;
		while (i!=binaries.Length) {			
			output += binaryToChar(binaries.Substring(i,8));
			i+=8;
		}
		
		return output;
	}
	
	public char xorBit(char _bit1,char _bit2) 
	/* does xor operator with bits in char types */
	{
		return ((_bit1==_bit2) ? '0':'1') ;
	}
	
	public void encipher()
	/* enciphers with ECB method */
	{
		string inputBit = this.inputText_;
		int inputBitLength = inputBit.Length;
		String key = this.key_;
		int blockLength = key.Length;
		String blockTemp = "";
		String output = "";
		String outputText = "";
		int i = 0;
		int j = 0;
		
		while (inputBit.Length % blockLength != 0) {
			inputBit += '0';
		}
		
		while (i != inputBit.Length) {
			
			for (j=0;j!=blockLength;j++) {
				blockTemp += inputBit[j+i];
			}
			
			output += encryptionBlock(blockTemp);
			blockTemp = "";
			j = 0;
			i += blockLength;
		}
		
		outputText = binaryToString(output.Substring(0,inputBitLength));
		this.outputText_ = outputText;
		this.outputBytes_ = binaryStringToBytes(output.Substring(0,inputBitLength));

		Console.WriteLine("\n>Encryption Result:");
		Console.WriteLine(outputText);
	}
	
	public void decipher()
	/* deciphers with ECB method */
	{
		string inputBit = this.inputText_;
		int inputBitLength = inputBit.Length;
		String key = this.key_;
		int blockLength = key.Length;
		String blockTemp = "";
		String output = "";
		String outputText = "";
		int i = 0;
		int j = 0;
		
		while (inputBit.Length % blockLength != 0) {
			inputBit += '0';
		}
		
		while (i != inputBit.Length) {
			
			for (j=0;j!=blockLength;j++) {
				blockTemp += inputBit[j+i];
			}
			
			output += decryptionBlock(blockTemp);
			blockTemp = "";
			j = 0;
			i += blockLength;
		}
		
		outputText = binaryToString(output.Substring(0,inputBitLength));
		this.outputBytes_ = binaryStringToBytes(output.Substring(0,inputBitLength));
		this.outputText_ = outputText;
		
		Console.WriteLine("\n>Decryption Result:");
		Console.WriteLine(outputText);
	}
	
	public void loadFile()
	/* loads input from a file */
	{
		string fileString = "";
		UTF8Encoding utf8 = new UTF8Encoding();
		
		try {			
			Console.Write("\nInput file name: ");
			fileString = Console.ReadLine();
			Console.WriteLine();
			string output = "";			
			byte[] bytes = File.ReadAllBytes(fileString);			
			BitArray bits = new BitArray(bytes);
	
			for (int i=0;i!=bits.Length;i+=1) {
				output += (bits[i] ? '1' : '0');
			}
			
			String inputText = utf8.GetString(bytes);
			Console.WriteLine(inputText);
			this.inputText_ = output;
			
		} catch (Exception) {
			Console.WriteLine("> File not found. Try again.");
			loadFile();
		}
	}
	
	public void saveFile(int _index)
	/* saves output into a file */
	{	
		if (_index == 0) {
			try {
				File.WriteAllBytes("output_encipher.txt", this.outputBytes_);				
				Console.WriteLine("\n> ECB enciphering result has been saved into output_encipher.txt");
			
			} catch (Exception) {
				Console.WriteLine("> File not found.");
			}
		} else if (_index == 1) {
			try {
				File.WriteAllBytes("output_decipher.txt", this.outputBytes_);				
				Console.WriteLine("\n> ECB deciphering result has been saved into output_dncipher.txt");
			
			} catch (Exception) {
				Console.WriteLine("> File not found.");
			}
		}
	}
	
	
    // DRIVER

    public void driver()
    /* simulates instance functionality */
    {
		menu();
    }

    public static void Main(String[] args)
    /* executes class program */
    {
		
        ECB ecb = new ECB();
        
		Console.Write("\n===== ECB (Electronic Code Book) =====\n");
		Console.Write("\n\n== START PROGRAM ==\n\n");
		ecb.driver();
		Console.Write("\n\n== END PROGRAM ==\n\n");
		Console.ReadLine();
    }
	
}