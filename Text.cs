namespace PublicUtility.TextFileManager {

  public class Text: IDisposable {
    private readonly string _filePath;
    private readonly string _lineDelimiter;

    public Text(string filePath, string lineDelimiter) {
      _filePath = filePath;
      _lineDelimiter = lineDelimiter;

      if(!Exists(_filePath))
        Create(_filePath, "");
    }

    private Text(string filePath) {
      _filePath = filePath;
      _lineDelimiter = "\r\n";

      if(!Exists(_filePath))
        Create(_filePath, "");
    }

    #region PRIVATE METHODS

    private IList<string> SelectText(Filter filter = null) {
      var lines = File.ReadAllText(_filePath).Split(_lineDelimiter).Where(x => x != string.Empty).ToList();

      if(filter == null)
        return lines;

      if(filter._uniqueLineNumber >= 0)
        return new List<string> { lines[filter._uniqueLineNumber - 1] };

      if(filter._endIndex > 0 && filter._startIndex > 0) {
        if(lines.Count < filter._startIndex)
          return new List<string>();

        var response = new List<string>();
        for(int id = 0; id < lines.Count; id++) {
          if(id >= filter._startIndex - 1 && id <= filter._endIndex - 1)
            response.Add(lines[id]);
        }
        return response;
      }

      if(filter._multiLines != null) {
        var response = new List<string>();
        for(int id = 0; id < lines.Count; id++) {
          if(filter._multiLines.Contains(id))
            response.Add(lines[id - 1]);
        }

        return response;
      }

      if(!string.IsNullOrEmpty(filter._lineFragment)) {
        var response = new List<string>();

        foreach(var line in lines) {
          if(line.Contains(filter._lineFragment))
            response.Add(line);
        }

        return response;
      }

      return new List<string>();
    }

    private void DeleteText(Filter filter = null) {
      var lines = Select();

      if(filter == null) {
        Clear();
        return;
      }

      if(filter._uniqueLineNumber > 0) {
        var newLines = new List<string>();
        for(int i = 0; i < lines.Count; i++) {
          if(i == filter._uniqueLineNumber - 1)
            continue;

          newLines.Add(lines[i]);
        }

        Clear();
        Insert(newLines);

        return;
      }

      if(filter._endIndex > 0 && filter._startIndex > 0) {
        var newLines = new List<string>();
        for(int id = 0; id < lines.Count; id++) {
          if(id >= filter._startIndex - 1 && id <= filter._endIndex - 1)
            continue;
          newLines.Add(lines[id]);
        }

        Clear();
        Insert(newLines);
        return;
      }

      if(filter._multiLines != null) {
        var newLines = new List<string>();
        for(int id = 1; id <= lines.Count; id++) {
          if(filter._multiLines.Contains(id))
            continue;
          newLines.Add(lines[id - 1]);
        }

        Clear();
        Insert(newLines);

        return;
      }
    }

    private void ReplaceText(Filter filter) {
      var textLines = Select();
      bool change = false;

      #region REPLACE TEXTLINE FOR TEXTLINE

      if(!string.IsNullOrEmpty(filter._oldTextLine)) {
        for(int i = 0; i < textLines.Count; i++) {

          if(textLines[i] == filter._oldTextLine) {
            change = true;
            textLines[i] = filter._newTextLine;

            if(!filter._allMatches)
              break;
          }
        }

        if(change) {
          Clear();
          Insert(textLines);
        }

        return;
      }

      #endregion

      #region REPLACE TEXLINE BY INDEX OF LINE

      if(filter._oldTextLineIndex > 0) {
        for(int i = 0; i < textLines.Count; i++) {

          if(i + 1 == filter._oldTextLineIndex) {
            change = true;
            textLines[i] = filter._newTextLine;

            break;
          }
        }

        if(change) {
          Clear();
          Insert(textLines);
        }

        return;
      }

      #endregion
    }

    #endregion
    
    public void Dispose() {
      GC.Collect();
      GC.SuppressFinalize(this);
    }

    public static Text LoadFile(string filePath) => new(filePath);

    public static Text LoadFile(string filePath, string lineDelimiter) => new(filePath, lineDelimiter);

    public static bool Exists(string filePath) => File.Exists(filePath);

    public static void Create(string filePath, string text) => File.WriteAllText(filePath, text);

    public static void Delete(string filePath) => File.Delete(filePath);

    public static string Select(string filePath) => File.ReadAllText(filePath);

    public void Clear() => File.WriteAllText(_filePath, "");

    #region OVERLOAD INSERT
    
    public void Insert(string newTextLine) => File.AppendAllText(_filePath, newTextLine.Replace("\n", "").Replace("\r", "") + _lineDelimiter);

    public void Insert(IEnumerable<string> lines) {
      int i = 0;
      foreach(string line in lines) {
        i++;

        if(i == lines.Count() && string.IsNullOrEmpty(line)) // ignore blank line in end text
          continue;

        Insert(line);
      }

    }
    
    #endregion

    #region OVERLOAD DELETE

    public void Delete(int lineNumber) => DeleteText(new Filter(lineNumber));

    public void Delete(int startAt, int finishIn) => DeleteText(new Filter(startAt, finishIn));
    
    public void Delete(IList<int> multiLines) => DeleteText(new Filter(multiLines));
    
    public void Delete(int[] multilines) => DeleteText(new Filter(multilines));

    public void Delete() => DeleteText();

    #endregion

    #region OVERLOAD REPLACE

    public void Replace(string oldTextLine, string newTextLine, bool allMatches) => ReplaceText(new Filter(newTextLine, oldTextLine, allMatches));
 
    public void Replace(int oldTextLineNumber, string newTextLine) => ReplaceText(new Filter(newTextLine, oldTextLineNumber));

    #endregion

    #region OVERLOAD SELECT

    public string Select(int lineNumber) => SelectText(new Filter(lineNumber)).FirstOrDefault();
    
    public IList<string> Select(string lineFragment) => SelectText(new Filter(lineFragment));

    public IList<string> Select(int startAt, int finishIn) => SelectText(new Filter(startAt, finishIn));

    public IList<string> Select(IList<int> multilines) => SelectText(new Filter(multilines));

    public IList<string> Select(int[] multilines) => SelectText(new Filter(multilines));

    public IList<string> Select() => SelectText();
    #endregion

  }
}