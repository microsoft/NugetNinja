// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin
{
    public class MissingProperty : IAction
    {
        private readonly Project _csproj;
        private readonly string _propertyName;
        private readonly string? _currentValue;
        private readonly string _suggestedValue;

        public MissingProperty(Project csproj, string propertyName, string suggestedValue, string? currentValue = null)
        {
            _csproj = csproj;
            _propertyName = propertyName;
            _suggestedValue = suggestedValue;
            _currentValue = currentValue;
        }

        public string BuildMessage()
        {
            if (string.IsNullOrWhiteSpace(_currentValue))
            {
                return $"The project: '{_csproj}' seems to be a library that might be shared. But lack of property '{_propertyName}'. You can possibly set that to: '{_suggestedValue}'.";
            }
            else
            {
                return $"The project: '{_csproj}' seems to be a library that might be shared. But property '{_propertyName}' with value '{_currentValue}' was not suggested. You can possibly set that to: '{_suggestedValue}'.";
            }
        }

        public Task TakeActionAsync()
        {
            return _csproj.AddOrUpdateProperty(_propertyName, _suggestedValue);
        }
    }
}
