namespace PublicUtility.TextFileManager {
  internal class Filter {

    internal readonly int _startIndex = -1;
    internal readonly int _endIndex = -1;
    internal readonly int _uniqueLineNumber = -1;
    internal readonly IList<int> _multiLines = null;
    internal readonly string _newTextLine = string.Empty;
    internal readonly string _oldTextLine = string.Empty;
    internal readonly string _lineFragment = string.Empty;
    internal readonly bool _allMatches = true;
    internal readonly int _oldTextLineIndex = -1;

    internal Filter(int startIndex, int endIndex) { 
      _endIndex = endIndex;
      _startIndex = startIndex;
    }

    internal Filter(int uniqueLineNumber) {
      _uniqueLineNumber = uniqueLineNumber;
    }

    internal Filter(IList<int> multiLines) {
      _multiLines = multiLines;
    }

    internal Filter(string newTextLine, int oldTextLineIndex) {
      _newTextLine = newTextLine;
      _oldTextLineIndex = oldTextLineIndex;
    }

    internal Filter(string newTextLine, string oldTextLine, bool allMatches) {
      _newTextLine = newTextLine;
      _oldTextLine = oldTextLine;
      _allMatches = allMatches;
    }

    internal Filter(string lineFragment) {
      _lineFragment = lineFragment;
    }
  }
}
