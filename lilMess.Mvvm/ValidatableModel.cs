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
            var handler = this.ErrorsChanged;
            if (handler != null) handler(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            this.errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        protected override void RaisePropertyChanged(string propertyName = null)
        {
            this.Validate();

            base.RaisePropertyChanged(propertyName);
        }

        public bool HasErrors
        {
            get { return this.errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public void Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, validationResults, true);

            var keyValuePairs = this.errors.ToList();

            foreach (var valuePair in keyValuePairs.Where(keyValuePair => validationResults.All(validationResult => validationResult.MemberNames.All(memberName => memberName != keyValuePair.Key))))
            {
                List<string> outputList;
                this.errors.TryRemove(valuePair.Key, out outputList);
                this.OnErrorsChanged(valuePair.Key);
            }

            var q = from r in validationResults from m in r.MemberNames group r by m into g select g;

            foreach (var prop in q)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();

                if (this.errors.ContainsKey(prop.Key))
                {
                    List<string> outLi;
                    this.errors.TryRemove(prop.Key, out outLi);
                }

                this.errors.TryAdd(prop.Key, messages);
                this.OnErrorsChanged(prop.Key);
            }
        }
    }
}