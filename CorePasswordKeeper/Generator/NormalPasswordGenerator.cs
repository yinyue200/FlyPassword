using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace CorePasswordKeeper.Generator
{
    /// <summary>
    /// Holds all of the settings for the password generator
    /// </summary>
    public class PasswordGeneratorSettings
    {
        const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
        const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMERIC_CHARACTERS = "0123456789";
        const string SPECIAL_CHARACTERS = @"!#$%&*@\";
        int _defaultMinPasswordLength = 8;
        int _defaultMaxPasswordLength = 128;

        public bool IncludeLowercase { get; set; }
        public bool IncludeUppercase { get; set; }
        public bool IncludeNumeric { get; set; }
        public bool IncludeSpecial { get; set; }
        public int PasswordLength { get; set; }
        public string CharacterSet { get; set; }
        public int MaximumAttempts { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
        public bool UsingDefaults { get; set; }

        public PasswordGeneratorSettings(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, int passwordLength, int maximumAttempts, bool usingDefaults)
        {
            IncludeLowercase = includeLowercase;
            IncludeUppercase = includeUppercase;
            IncludeNumeric = includeNumeric;
            IncludeSpecial = includeSpecial;
            PasswordLength = passwordLength;
            MaximumAttempts = maximumAttempts;
            MinimumLength = _defaultMinPasswordLength;
            MaximumLength = _defaultMaxPasswordLength;
            UsingDefaults = usingDefaults;
            CharacterSet = BuildCharacterSet(includeLowercase, includeUppercase, includeNumeric, includeSpecial);
        }

        private string BuildCharacterSet(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial)
        {
            StringBuilder characterSet = new StringBuilder();
            if (includeLowercase)
            {
                characterSet.Append(LOWERCASE_CHARACTERS);
            }

            if (includeUppercase)
            {
                characterSet.Append(UPPERCASE_CHARACTERS);
            }

            if (includeNumeric)
            {
                characterSet.Append(NUMERIC_CHARACTERS);
            }

            if (includeSpecial)
            {
                characterSet.Append(SPECIAL_CHARACTERS);
            }
            return characterSet.ToString();
        }

        public PasswordGeneratorSettings AddLowercase()
        {
            StopUsingDefaults();
            IncludeLowercase = true;
            CharacterSet += LOWERCASE_CHARACTERS;
            return this;
        }

        public PasswordGeneratorSettings AddUppercase()
        {
            StopUsingDefaults();
            IncludeUppercase = true;
            CharacterSet += UPPERCASE_CHARACTERS;
            return this;
        }

        public PasswordGeneratorSettings AddNumeric()
        {
            StopUsingDefaults();
            IncludeNumeric = true;
            CharacterSet += NUMERIC_CHARACTERS;
            return this;
        }

        public PasswordGeneratorSettings AddSpecial()
        {
            StopUsingDefaults();
            IncludeSpecial = true;
            CharacterSet += SPECIAL_CHARACTERS;
            return this;
        }

        private void StopUsingDefaults()
        {
            if (UsingDefaults)
            {
                CharacterSet = string.Empty;
                IncludeLowercase = false;
                IncludeUppercase = false;
                IncludeNumeric = false;
                IncludeSpecial = false;
                UsingDefaults = false;
            }
        }
    }
    public class NormalPasswordGenerator : IPasswordGenerator
    {
        public string Generate()
        {
            return Next();
        }
        private int _defaultPasswordLength = 16;
        private int _defaultMaxPasswordAttempts = 10000;
        private bool _defaultIncludeLowercase = true;
        private bool _defaultIncludeUppercase = true;
        private bool _defaultIncludeNumeric = true;
        private bool _defaultIncludeSpecial = true;

        private PasswordGeneratorSettings _settings { get; set; }

        public NormalPasswordGenerator()
        {
            _settings = new PasswordGeneratorSettings(_defaultIncludeLowercase, _defaultIncludeUppercase, _defaultIncludeNumeric, _defaultIncludeSpecial, _defaultPasswordLength, _defaultMaxPasswordAttempts, true);
        }

        public NormalPasswordGenerator(PasswordGeneratorSettings settings)
        {
            _settings = settings;
        }

        public NormalPasswordGenerator(int passwordLength)
        {
            _settings = new PasswordGeneratorSettings(_defaultIncludeLowercase, _defaultIncludeUppercase, _defaultIncludeNumeric, _defaultIncludeSpecial, passwordLength, _defaultMaxPasswordAttempts, true);
        }

        public NormalPasswordGenerator(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial)
        {
            _settings = new PasswordGeneratorSettings(includeLowercase, includeUppercase, includeNumeric, includeSpecial, _defaultPasswordLength, _defaultMaxPasswordAttempts, false);
        }

        public NormalPasswordGenerator(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, int passwordLength)
        {
            _settings = new PasswordGeneratorSettings(includeLowercase, includeUppercase, includeNumeric, includeSpecial, passwordLength, _defaultMaxPasswordAttempts, false);
        }

        public NormalPasswordGenerator(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, int passwordLength, int maximumAttempts)
        {
            _settings = new PasswordGeneratorSettings(includeLowercase, includeUppercase, includeNumeric, includeSpecial, passwordLength, maximumAttempts, false);
        }

        public NormalPasswordGenerator IncludeLowercase()
        {
            this._settings = _settings.AddLowercase();
            return this;
        }

        public NormalPasswordGenerator IncludeUppercase()
        {
            this._settings = _settings.AddUppercase();
            return this;
        }

        public NormalPasswordGenerator IncludeNumeric()
        {
            this._settings = _settings.AddNumeric();
            return this;
        }

        public NormalPasswordGenerator IncludeSpecial()
        {
            this._settings = _settings.AddSpecial();
            return this;
        }

        public NormalPasswordGenerator LengthRequired(int passwordLength)
        {
            this._settings.PasswordLength = passwordLength;
            return this;
        }

        /// <summary>
        /// Gets the next random password which meets the requirements
        /// </summary>
        /// <returns>A password as a string</returns>
        public string Next()
        {
            string password;
            if (!LengthIsValid(_settings.PasswordLength, _settings.MinimumLength, _settings.MaximumLength))
            {
                password = string.Format("Password length invalid. Must be between {0} and {1} characters long", _settings.MinimumLength, _settings.MaximumLength);
            }
            else
            {
                int passwordAttempts = 0;
                do
                {
                    password = GenerateRandomPassword(_settings);
                    passwordAttempts++;
                }
                while (passwordAttempts < _settings.MaximumAttempts && !PasswordIsValid(_settings, password));

                password = PasswordIsValid(_settings, password) ? password : "Try again";
            }

            return password;
        }


        /// <summary>
        /// Generates a random password based on the rules passed in the settings parameter
        /// This does not do any validation
        /// </summary>
        /// <param name="settings">Password generator settings object</param>
        /// <returns>a random password</returns>
        private string GenerateRandomPassword(PasswordGeneratorSettings settings)
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            char[] password = new char[settings.PasswordLength];

            char[] characters = settings.CharacterSet.ToCharArray();
            char[] shuffledChars = Shuffle(characters.Select(x => x)).ToArray();

            string shuffledCharacterSet = string.Join(null, shuffledChars);
            int characterSetLength = shuffledCharacterSet.Length;

            System.Random random = new System.Random();
            for (int characterPosition = 0; characterPosition < settings.PasswordLength; characterPosition++)
            {
                password[characterPosition] = shuffledCharacterSet[random.Next(characterSetLength - 1)];

                bool moreThanTwoIdenticalInARow =
                    characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                    && password[characterPosition] == password[characterPosition - 1]
                    && password[characterPosition - 1] == password[characterPosition - 2];

                if (moreThanTwoIdenticalInARow)
                {
                    characterPosition--;
                }
            }

            return string.Join(null, password);
        }

        /// <summary>
        /// When you give it a password and some _settings, it validates the password against the _settings.
        /// </summary>
        /// <param name="settings">Password settings</param>
        /// <param name="password">Password to test</param>
        /// <returns>True or False to say if the password is valid or not</returns>
        public bool PasswordIsValid(PasswordGeneratorSettings settings, string password)
        {
            const string REGEX_LOWERCASE = @"[a-z]";
            const string REGEX_UPPERCASE = @"[A-Z]";
            const string REGEX_NUMERIC = @"[\d]";
            const string REGEX_SPECIAL = @"([!#$%&*@\\])+";

            bool lowerCaseIsValid = !settings.IncludeLowercase || (settings.IncludeLowercase && Regex.IsMatch(password, REGEX_LOWERCASE));
            bool upperCaseIsValid = !settings.IncludeUppercase || (settings.IncludeUppercase && Regex.IsMatch(password, REGEX_UPPERCASE));
            bool numericIsValid = !settings.IncludeNumeric || (settings.IncludeNumeric && Regex.IsMatch(password, REGEX_NUMERIC));
            bool specialIsValid = !settings.IncludeSpecial || (settings.IncludeSpecial && Regex.IsMatch(password, REGEX_SPECIAL));

            return lowerCaseIsValid && upperCaseIsValid && numericIsValid && specialIsValid && LengthIsValid(password.Length, settings.MinimumLength, settings.MaximumLength);
        }

        private bool CharacterSetIsValid(bool includeThisCharacterSet, string password, string charactersetRegex)
        {
            return !includeThisCharacterSet || (includeThisCharacterSet && Regex.IsMatch(password, charactersetRegex));
        }

        /// <summary>
        /// Checks that the password is within the valid length range
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>A bool to say if it is valid or not</returns>
        private bool LengthIsValid(int passwordLength, int minLength, int maxLength)
        {
            return passwordLength >= minLength && passwordLength <= maxLength;
        }

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> items)
        {
            return from item in items orderby System.Guid.NewGuid() ascending select item;
        }
    }
}
