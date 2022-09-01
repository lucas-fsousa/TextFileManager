# TextFile Manager

It provides auxiliary methods for data processing via text document, ideal for local log files where information is needed to be identified precisely without harming the archiving flow.

## Installation

To install, just run the C# compiler to generate the .dll file and once the file has been generated, just add the reference to the project or use [Nuget](https://www.nuget.org/packages/PublicUtility.TextFileManager) or in nuget console, use the following command:

```bash
install-Package PublicUtility.TextFileManager
```
## Startup
There are two ways to initialize the text file that are used according to the user's need or the way the text is divided.

- Load(string filePath)
- Load(string filePath, string lineDelimiter)

The delimiter is very important to know how the methods will recognize the lines. Usually the texts show a line break with "\r\n", however there may be other forms of separation such as: ";", "|", "-", "/", "." and etc.

Use the base call by just passing the file path if your file doesn't have line separations other than "\r\n" or use the alternate call specifying how the lines should be treated in the delimiter.

#### Example 1:
```csharp
using PublicUtility.TextFileManager;

// Simple call showing only the file path and defaulting to the line delimiter in the form of "\r\n"
using var text = Text.Load(@"C:\MyDocs\base.log"); 

```

#### Example 2:
```csharp
using PublicUtility.TextFileManager;

// Alternative call informing the file path and the delimiter of the lines in the format of ";"
using var text = Text.Load(@"C:\MyDocs\base.log", ";");

```

## Methods

### Insert

It can be used to insert lines of text or the entire text given the user's need.

- Insert(string newTextLine)
- Insert(IEnumerable<string> lines)
- static Create(string filePath, string text)

#### How to use

```csharp
using PublicUtility.TextFileManager;

using var text = Text.Load(@"C:\MyDocs\base.log");
var lines = new List<string> { "ROW 2 ADDED IN MULTLINES", "ROW 3 ADDED IN MULTLINES", "ROW 4 ADDED IN MULTLINES" };

text.Insert($"ROW 1 ADDED"); // ADD A SINGLE TEXT LINE TO THE LOADED TEXT DOCUMENT
text.Insert(lines); // ADD MULTIPLE LINES TO THE LOADED TEXT DOCUMENT

// USING A STATIC CREATE
Text.Create(@"C:\MyDocs\text.txt", "THIS IS A FULL TEXT LINES"); // CREATES A TEXT FILE AT THE TARGET LOCATION

```

### Delete

Used to delete the text according to the user's need, giving more freedom and precision to the manipulation of documents in text format.

- Delete(int lineNumber)
- Delete(int[] multilines)
- Delete(IList<int> multiLines)
- Delete(int startAt, int finishIn)
- static Delete(string filePath)

#### How to use

```csharp
using PublicUtility.TextFileManager;

using var text = Text.Load(@"C:\MyDocs\base.log");
text.Delete(10); // DELETES ONLY THE SPECIFIED LINE OF TEXT IF IT EXISTS.
text.Delete(1, 5); // DELETES ALL LINES IN A RANGE. EXAMPLE: STARTING FROM LINE 1 TO LINE 5
text.Delete(new List<int> { 1, 2, 5, 10, 16 }); // DELETE MULTIPLE SPECIFIED LINES ACCORDING TO THEIR ENUMERATION.
text.Delete(new int[] { 1, 2, 5, 10, 16 }); // DELETE MULTIPLE SPECIFIED LINES ACCORDING TO THEIR ENUMERATION.

// USING A STATIC DELETE
Text.Delete(@"C:\MyDocs\text.txt"); // DELETES THE TEXT FILE ITSELF AND NOT THE TEXT IN THE FILE.

```

### Select

Used to select text or parts of it for precise manipulation according to the call used.

- Select()
- Select(int lineNumber)
- Select(int[] multilines)
- Select(string lineFragment)
- Select(IList<int> multiLines)
- Select(int startAt, int finishIn)
- static GetText(string filePath)

#### How to use

```csharp
using PublicUtility.TextFileManager;

using var text = Text.Load(@"C:\MyDocs\base.log");
text.Select(); // GET ALL LINES FROM THE TEXT DOCUMENT
text.Select("ONE TEXT"); // GET ALL LINES THAT MATCH THE GIVEN TERM
text.Select(10); // GET A ROW BASED ON THE INDEX
text.Select(1, 20); // GET ALL ROWS IN A SPECIFIED RANGE
text.Select(new List<int> { 1, 5, 10, 15, 20 }); // GET SPECIFIC ROWS BASED ON INDEX
text.Select(new int[] { 1, 5, 10, 15, 20 }); // GET SPECIFIC ROWS BASED ON INDEX

// USING A STATIC SELECT
Text.GetText(@"C:\MyDocs\text.txt"); // GET PLAIN TEXT
```


### Replace

It can be used for precise replacement of values ​​anywhere in a text as long as the line break fits.

- Replace(string oldTextLine, string newTextLine, bool allMatches)
- Replace(int oldTextLineNumber, string newTextLine)

#### How to use
```csharp
using PublicUtility.TextFileManager;

using var text = Text.Load(@"C:\MyDocs\base.log");
text.Replace(10, "THANKS FOR READ THIS DOCUMENTATION xD"); // REPLACES THE TEXT LINE 10 WITH A NEW VALUE.
text.Replace("THANKS FOR READ THIS DOCUMENTATION xD", "LOOK FOR MORE ABOUT PUBLICUTILITY!", false); // REPLACES ONLY THE FIRST LINE THAT MATCHES THE REQUEST.
text.Replace("THANKS FOR READ THIS DOCUMENTATION xD", "LOOK FOR MORE ABOUT PUBLICUTILITY!", true); // REPLACES ALL LINES THAT MATCH THE REQUEST.
```

### Dispose

- Dispose()

If the "using" directive is being used at object initialization, the "Dispose" method will be invoked intelligently at the end of the instantiated object's use. If the "using" directive is not being used, the "Dispose" method can be invoked whenever the user finds it necessary.

#### Dispose use example
```csharp
using PublicUtility.TextFileManager;

// using the "using" directive
using var text = Text.Load(@"C:\MyDocs\base.log");
text.Replace(1, "Hello!");
text.Select();
text.Insert("HELLO WORLD!"); // Dispose called automatically after the last use of the object.

// without using the "using" directive
var text = Text.Load(@"C:\MyDocs\base.log");
text.Replace(1, "Hello!");
text.Select();
text.Insert("HELLO WORLD!");
text.Dispose(); // call dispose here

```
### Clear

The clear method is used to delete all existing records in the document and keep it alive in the system just empty.

- Clear()

#### How to use
```csharp
using PublicUtility.TextFileManager;

var text = Text.Load(@"C:\MyDocs\base.log");
text.Insert("HELLO");
text.Insert("WORLD!");
text.Clear(); // deletes all text in the document.
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)