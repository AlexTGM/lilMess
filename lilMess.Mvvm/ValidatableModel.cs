namespace lilMess.Mvvm
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using GalaSoft.MvvmLight;

    public class ValidatableModel : ViewModelBase, INotifyDataErrorInfo
    {
        private ConcurrentDictionary<string, List<string>> errors = new ConcurrentDictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        protected override void RaisePropertyChanged(string propertyName = null)
        {
            Validate();

            base.RaisePropertyChanged(propertyName);
        }

        public bool HasErrors
        {
            get { return errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public void Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, validationResults, true);

            var keyValuePairs = errors.ToList();

            foreach (var valuePair in keyValuePairs.Where(keyValuePair => validationResults.All(validationResult => validationResult.MemberNames.All(memberName => memberName != keyValuePair.Key))))
            {
                List<string> outputList;
                errors.TryRemove(valuePair.Key, out outputList);
                OnErrorsChanged(valuePair.Key);
            }

            var q = from r in validationResults from m in r.MemberNames group r by m into g select g;

            foreach (var prop in q)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();

                if (errors.ContainsKey(prop.Key))
                {
                    List<string> outLi;
                    errors.TryRemove(prop.Key, out outLi);
                }

                errors.TryAdd(prop.Key, messages);
                OnErrorsChanged(prop.Key);
            }
        }
    }
}