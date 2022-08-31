﻿namespace PublicUtility.TextFileManager {
  [Flags]
  public enum ActionType {
    Delete,
    Select
  }

  public class Text {

    #region PRIVATE METHODS

    private static List<string> TextWhere(string filePath, string lineDelimiter, ActionType actionType, int uniqueLineNumber = 0, int startIndex = 0, int endIndex = 0, List<int> multiLines = null, bool allLines = false) {
      List<string> response = new List<string>();
      List<string> lines = File.ReadAllText(filePath).Split(lineDelimiter).ToList();

      #region UNIQUE LINE NUMBER

      if(uniqueLineNumber > 0 && lines.Count >= uniqueLineNumber) {

        if(actionType == ActionType.Select) {
          response.Add(lines[uniqueLineNumber - 1]);

          return response;

        } else if(actionType == ActionType.Delete) {
          List<string> newLines = new List<string>();
          for(int i = 0; i < lines.Count; i++) {
            if(i == uniqueLineNumber - 1)
              continue;
            newLines.Add(lines[i]);
          }

          ClearText(filePath);
          AppendMultLines(filePath, newLines);
        }

      }

      #endregion

      #region RANGE LINE NUMBER

      if(startIndex > 0 && endIndex > 0) {
        if(lines.Count >= startIndex) {

          if(actionType == ActionType.Select) {
            for(int id = 0; id < lines.Count; id++) {
              if(id >= startIndex - 1 && id <= endIndex - 1)
                response.Add(lines[id]);
            }

            return response;

          } else if(actionType == ActionType.Delete) {
            List<string> newLines = new List<string>();
            for(int id = 0; id < lines.Count; id++) {
              if(id >= startIndex - 1 && id <= endIndex - 1)
                continue;
              newLines.Add(lines[id]);
            }

            ClearText(filePath);
            AppendMultLines(filePath, newLines);
          }

        }

      }

      #endregion

      #region EXPLICIT LINES

      if(multiLines != null) {

        if(actionType == ActionType.Select) {
          for(int id = 0; id < lines.Count; id++) {
            if(multiLines.Contains(id))
              response.Add(lines[id - 1]);
          }

          return response;

        } else if(actionType == ActionType.Delete) {

          List<string> newLines = new List<string>();
          for(int id = 1; id <= lines.Count; id++) {
            if(multiLines.Contains(id))
              continue;
            newLines.Add(lines[id - 1]);
          }

          ClearText(filePath);
          AppendMultLines(filePath, newLines);
        }

      }

      #endregion

      #region ALLLINES

      if(allLines) {
        if(actionType == ActionType.Select) {
          response = lines;
          return response;

        } else if(actionType == ActionType.Delete) {
          ClearText(filePath);

        }
      }

      #endregion

      return response;
    }

    private static void BaseReplaceLine(string filePath, string newTextLine, string delimiter, string oldTextLine = "", int oldTextLineIndex = 0, bool onlyFirstLocated = false) {
      string[] textLines = File.ReadAllText(filePath).Split(delimiter);
      bool change = false;

      #region REPLACE TEXTLINE FOR TEXTLINE

      if(!string.IsNullOrEmpty(oldTextLine)) {
        for(int i = 0; i < textLines.Length; i++) {

          if(textLines[i] == oldTextLine) {
            change = true;
            textLines[i] = newTextLine;

            if(onlyFirstLocated)
              break;
          }
        }

        if(change) {
          File.WriteAllText(filePath, "");
          AppendMultLines(filePath, textLines);
        }


        return;
      }

      #endregion

      #region REPLACE TEXLINE BY INDEX OF LINE

      if(oldTextLineIndex > 0) {
        for(int i = 0; i < textLines.Length; i++) {

          if(i + 1 == oldTextLineIndex) {
            change = true;
            textLines[i] = newTextLine;

            break;
          }
        }

        if(change) {
          File.WriteAllText(filePath, "");
          AppendMultLines(filePath, textLines);
        }

        return;
      }

      #endregion

    }

    #endregion

    public static void AppendLine(string filePath, string newTextLine) => File.AppendAllText(filePath, newTextLine.Replace("\n", "").Replace("\r", "") + "\r\n");

    public static void AppendMultLines(string filePath, IEnumerable<string> lines) {
      int i = 0;
      foreach(string line in lines) {
        i++;

        if(i == lines.Count() && string.IsNullOrEmpty(line)) // ignore blank line in end text
          continue;

        AppendLine(filePath, line);
      }

    }

    public static string GetText(string filePath) => File.ReadAllText(filePath);

    public static void CreateText(string filePath, string text) => File.WriteAllText(filePath, text);

    public static void ClearText(string filePath) => File.WriteAllText(filePath, "");

    public static bool Exists(string filePath) => File.Exists(filePath);


    #region OVERLOAD DELETELINES

    public static void DeleteLine(string filePath, int lineNumber, string lineDelimiter = "\r\n") => TextWhere(filePath, lineDelimiter, ActionType.Delete, uniqueLineNumber: lineNumber);

    public static void DeleteLines(string filePath, int startAt, int finishIn, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(startAt < 0)
        throw new Exception($"{nameof(startAt)} less than zero");

      if(finishIn < 0)
        throw new Exception($"{nameof(finishIn)} less than zero");

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      TextWhere(filePath, lineDelimiter, ActionType.Delete, startIndex: startAt, endIndex: finishIn);
    }

    public static void DeleteLines(string filePath, List<int> SpecificLines, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(SpecificLines.Count < 0)
        throw new Exception($"{nameof(SpecificLines)} is null or empty");

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      TextWhere(filePath, lineDelimiter, ActionType.Delete, multiLines: SpecificLines);
    }

    public static void DeleteLines(string filePath, int[] SpecificLines, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(SpecificLines.Length < 0)
        throw new Exception($"{nameof(SpecificLines)} is null or empty");

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      TextWhere(filePath, lineDelimiter, ActionType.Delete, multiLines: SpecificLines.ToList());
    }

    /// <summary>
    /// [EN]: Delete all the lines of text belonging to the line indicators contained in the array or list<br></br>
    /// [PT-BR]: Deleta todas as linhas de texto pertencentes aos indicadores de linha contidos na matriz ou lista 
    /// </summary>
    /// <param name="filePath">
    /// [EN]: File path for reading lines<br></br>
    /// [PT-BR]: Caminho do arquivo para a leitura das linhas
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <exception cref="Exception"></exception>
    public static void DeleteLines(string filePath, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      TextWhere(filePath, lineDelimiter, ActionType.Delete, allLines: true);
    }
    #endregion

    #region OVERLOAD REPLACELINE

    /// <summary>
    /// [EN]: Replaces a line of text in the target file<br></br>
    /// [PT-BR]: Substitui uma linha de texto no arquivo de destino
    /// </summary>
    /// <param name="filePath">
    /// [EN]: Text file location<br></br>
    /// [PT-BR]: Localização do arquivo de texto
    /// </param>
    /// <param name="oldTextLine">
    /// [EN]: Line of text to be replaced<br></br>
    /// [PT-BR]: Linha de texto a ser substituida
    /// </param>
    /// <param name="newTextLine">
    /// [EN]: Line of text to be included in the destination point<br></br>
    /// [PT-BR]: Linha de texto a ser incluida no ponto de destino
    /// </param>
    /// <param name="onlyFirstLocated">
    /// [EN]: Indicates whether only the first line found should be replaced<br></br>
    /// [PT-BR]: Indica se apenas a primeira linha encontrada deverá ser substituida
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <exception cref="Exception"></exception>
    public static void ReplaceLine(string filePath, string oldTextLine, string newTextLine, bool onlyFirstLocated, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      if(newTextLine == null)
        throw new Exception($"{nameof(newTextLine)} is null or empty");

      if(oldTextLine == null)
        throw new Exception($"{nameof(oldTextLine)} is null or empty");

      #endregion

      BaseReplaceLine(filePath, newTextLine, delimiter: lineDelimiter, oldTextLine, onlyFirstLocated: onlyFirstLocated);
    }

    /// <summary>
    /// [EN]: Replaces a line of text in the target file<br></br>
    /// [PT-BR]: Substitui uma linha de texto no arquivo de destino
    /// </summary>
    /// <param name="filePath">
    /// [EN]: File path for reading lines<br></br>
    /// [PT-BR]: Caminho do arquivo para a leitura das linhas
    /// </param>
    /// <param name="oldTextLineNumber">
    /// [EN]: Number representing the line to be replaced<br></br>
    /// [PT-BR]: Numero que representa a linha a ser substituida
    /// </param>
    /// <param name="newTextLine">
    /// [EN]: Line of text to be included in the destination point<br></br>
    /// [PT-BR]: Linha de texto a ser incluida no ponto de destino
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <exception cref="Exception"></exception>
    public static void ReplaceLine(string filePath, int oldTextLineNumber, string newTextLine, string lineDelimiter = "\r\n") {
      #region PARAM VALIDATION

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      if(oldTextLineNumber < 0)
        throw new Exception($"{nameof(oldTextLineNumber)} less than zero");

      #endregion

      BaseReplaceLine(filePath, newTextLine, delimiter: lineDelimiter, oldTextLineIndex: oldTextLineNumber);
    }

    #endregion

    #region OVERLOAD GETTEXTLINES

    /// <summary>
    /// [EN]: Get a specific line from a text file<br></br>
    /// [PT-BR]: Obtém uma linha especifica de um arquivo de texto
    /// </summary>
    /// <param name="filePath">
    /// [EN]: File path for reading lines<br></br>
    /// [PT-BR]: Caminho do arquivo para a leitura das linhas
    /// </param>
    /// <param name="lineNumber">
    /// [EN]: Number indicating the line to be selected<br></br>
    /// [PT-BR]: Numero que indica a linha a ser selecionada
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <returns>
    /// [EN]: Returns a string containing the line referring to the presented identifier<br></br>
    /// [PT-BR]: Retorna uma cadeia de caracteres contendo a linha referente ao identificador apresentado
    /// </returns>
    /// <exception cref="Exception"></exception>
    public static string GetTextLine(string filePath, int lineNumber, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(lineNumber < 0)
        throw new Exception($"{nameof(lineNumber)} less than zero");

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      return TextWhere(filePath, lineDelimiter, ActionType.Select, uniqueLineNumber: lineNumber).FirstOrDefault();
    }

    /// <summary>
    /// [EN]: Get all lines of text in the given range<br></br>
    /// [PT-BR]: Obtém todas as linhas de texto que estiverem no intervalo informado
    /// </summary>
    /// <param name="filePath">
    /// [EN]: File path for reading lines<br></br>
    /// [PT-BR]: Caminho do arquivo para a leitura das linhas
    /// </param>
    /// <param name="startAt">
    /// [EN]: Number indicating where to start collecting lines of text<br></br>
    /// [PT-BR]: Numero que indica onde começara a coleta de linhas de texto
    /// </param>
    /// <param name="finishIn">
    /// [EN]: Number indicating where the collection of lines of text will end<br></br>
    /// [PT-BR]: Numero que indica onde será encerrada a coleta de linhas de texto
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <returns>
    /// [EN]: Returns a list of lines of text with all items that fall within the given range<br></br>
    /// [PT-BR]: Retorna uma lista de linhas de texto com todos os itens que se encaixarem no intervalo informado
    /// </returns>
    /// <exception cref="Exception"></exception>
    public static List<string> GetTextLines(string filePath, int startAt, int finishIn, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(startAt < 0)
        throw new Exception($"{nameof(startAt)} less than zero");

      if(finishIn < 0)
        throw new Exception($"{nameof(finishIn)} less than zero");

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      return TextWhere(filePath, lineDelimiter, ActionType.Select, startIndex: startAt, endIndex: finishIn);
    }

    /// <summary>
    /// [EN]: Gets all the lines of text belonging to the line indicators contained in the array or list that appear<br></br>
    /// [PT-BR]: Obtém todas as linhas de texto pertencentes aos indicadores de linha contidos na matriz ou lista que constam
    /// </summary>
    /// <param name="filePath">
    /// [EN]: File path for reading lines<br></br>
    /// [PT-BR]: Caminho do arquivo para a leitura das linhas
    /// </param>
    /// <param name="SpecificLines">
    /// [EN]: Represents specific lines at different places in the text<br></br>
    /// [PT-BR]: Representa Linhas especificas em lugares distintos no texto
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <returns>
    /// [EN]: Returns a list of lines of text corresponding to the given line indicators<br></br>
    /// [PT-BR]: Retorna uma lista de linhas de texto correspondentes aos indicadores de linha informados
    /// </returns>
    /// <exception cref="Exception"></exception>
    public static List<string> GetTextLines(string filePath, List<int> SpecificLines, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(SpecificLines.Count < 0)
        throw new Exception($"{nameof(SpecificLines)} is null or empty");

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      return TextWhere(filePath, lineDelimiter, ActionType.Select, multiLines: SpecificLines);
    }

    /// <summary>
    /// [EN]: Gets all the lines of text belonging to the line indicators contained in the array or list that appear<br></br>
    /// [PT-BR]: Obtém todas as linhas de texto pertencentes aos indicadores de linha contidos na matriz ou lista que constam
    /// </summary>
    /// <param name="filePath">
    /// [EN]: File path for reading lines<br></br>
    /// [PT-BR]: Caminho do arquivo para a leitura das linhas
    /// </param>
    /// <param name="SpecificLines">
    /// [EN]: Represents specific lines at different places in the text<br></br>
    /// [PT-BR]: Representa Linhas especificas em lugares distintos no texto
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <returns>
    /// [EN]: Returns a array of lines of text corresponding to the given line indicators<br></br>
    /// [PT-BR]: Retorna uma matriz de linhas de texto correspondentes aos indicadores de linha informados
    /// </returns>
    /// <exception cref="Exception"></exception>
    public static List<string> GetTextLines(string filePath, int[] SpecificLines, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(SpecificLines.Length < 0)
        throw new Exception($"{nameof(SpecificLines)} is null or empty");

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      return TextWhere(filePath, lineDelimiter, ActionType.Select, multiLines: SpecificLines.ToList());
    }

    /// <summary>
    /// [EN]: Gets all the lines of text belonging to the line indicators contained in the array or list that appear<br></br>
    /// [PT-BR]: Obtém todas as linhas de texto pertencentes aos indicadores de linha contidos na matriz ou lista que constam
    /// </summary>
    /// <param name="filePath">
    /// [EN]: File path for reading lines<br></br>
    /// [PT-BR]: Caminho do arquivo para a leitura das linhas
    /// </param>
    /// <param name="lineDelimiter">
    /// [EN]: Character string used to slice text into lines.<br></br>
    /// [PT-BR]: Sequencia de caractere utilizada para fatiar o texto em linhas.
    /// </param>
    /// <returns>
    /// [EN]: Returns a array of lines of text corresponding to the given line indicators<br></br>
    /// [PT-BR]: Retorna uma matriz de linhas de texto correspondentes aos indicadores de linha informados
    /// </returns>
    /// <exception cref="Exception"></exception>
    public static List<string> GetTextLines(string filePath, string lineDelimiter = "\r\n") {

      #region PARAM VALIDATION

      if(string.IsNullOrEmpty(filePath))
        throw new Exception($"{nameof(filePath)} is null or empty");

      if(!File.Exists(filePath))
        throw new Exception($"{nameof(filePath)} not exsists");

      #endregion

      return TextWhere(filePath, lineDelimiter, ActionType.Select, allLines: true);
    }

    #endregion
  }
}