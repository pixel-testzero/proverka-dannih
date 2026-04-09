using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WpfValidation
{
    public class User : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        private readonly Dictionary<string, List<string>> _errors = new();

        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _age = string.Empty;
        private string _password = string.Empty;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); ValidateName(value); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); ValidateEmail(value); }
        }

        public string Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(nameof(Age)); ValidateAge(value); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); ValidatePassword(value); }
        }


        private void ValidateName(string value)
        {
            _errors.Remove(nameof(Name));
            if (string.IsNullOrWhiteSpace(value))
                AddError(nameof(Name), "Имя обязательно для заполнения.");
            else if (value.Trim().Length < 2)
                AddError(nameof(Name), "Имя должно содержать не менее 2 символов.");
            OnErrorsChanged(nameof(Name));
        }

        private void ValidateEmail(string value)
        {
            _errors.Remove(nameof(Email));
            if (string.IsNullOrWhiteSpace(value))
                AddError(nameof(Email), "Email обязателен для заполнения.");
            else if (!Regex.IsMatch(value, @"^[\w\-\.]+@([\w\-]+\.)+[\w\-]{2,4}$"))
                AddError(nameof(Email), "Неверный формат email (пример: user@mail.ru).");
            OnErrorsChanged(nameof(Email));
        }

        private void ValidateAge(string value)
        {
            _errors.Remove(nameof(Age));
            if (string.IsNullOrWhiteSpace(value))
                AddError(nameof(Age), "Возраст обязателен для заполнения.");
            else if (!int.TryParse(value, out int age))
                AddError(nameof(Age), "Возраст должен быть целым числом.");
            else if (age < 1 || age > 120)
                AddError(nameof(Age), "Возраст должен быть в диапазоне от 1 до 120.");
            OnErrorsChanged(nameof(Age));
        }

        private void ValidatePassword(string value)
        {
            _errors.Remove(nameof(Password));
            if (string.IsNullOrWhiteSpace(value))
                AddError(nameof(Password), "Пароль обязателен для заполнения.");
            else if (value.Length < 6)
                AddError(nameof(Password), "Пароль должен содержать не менее 6 символов.");
            else if (!Regex.IsMatch(value, @"[A-Z]"))
                AddError(nameof(Password), "Пароль должен содержать хотя бы одну заглавную букву.");
            OnErrorsChanged(nameof(Password));
        }

        public void ValidateAll()
        {
            ValidateName(_name);
            ValidateEmail(_email);
            ValidateAge(_age);
            ValidatePassword(_password);
        }


        private void AddError(string propertyName, string message)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();
            _errors[propertyName].Add(message);
        }


        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void OnErrorsChanged(string propertyName) =>
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.Values.SelectMany(e => e);
            return _errors.TryGetValue(propertyName, out var errors)
                ? errors
                : Enumerable.Empty<string>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}