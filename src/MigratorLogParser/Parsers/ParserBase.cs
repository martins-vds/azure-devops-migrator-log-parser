using System.Text.RegularExpressions;

namespace MigratorLogParser.Parsers
{
    public abstract class ParserBase<T> : IParser<T> where T : new()
    {
        private readonly RegexOptions _regexOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase;
        protected string Pattern { get; }

        protected ParserBase(string pattern)
        {
            ArgumentNullException.ThrowIfNull(pattern, nameof(pattern));
            Pattern = pattern;
        }

        public virtual bool TryParse(string text, out T issue)
        {
            throw new NotImplementedException();
        }

        protected virtual bool IsMatch(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return false; }

            return Regex.IsMatch(text, Pattern, _regexOptions);
        }

        protected virtual bool IsMatch(string text, string pattern)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(pattern)) { return false; }

            return Regex.IsMatch(text, pattern, _regexOptions);
        }

        protected virtual Match Match(string text)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));
            return Regex.Match(text, Pattern, _regexOptions);
        }

        protected virtual Match Match(string text, string pattern)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));
            ArgumentNullException.ThrowIfNull(pattern, nameof(pattern));

            return Regex.Match(text, pattern, _regexOptions);
        }

        public virtual bool ShouldParse(string text)
        {
            throw new NotImplementedException();
        }

        protected int ParseInt(string text)
        {
            if (int.TryParse(text, out int result)) { return result; }

            return -1;
        }
    }

    public interface IParser<T>
    {
        bool TryParse(string text, out T issue);
    }
}