// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin
{
    public class MissingProperty : IAction
    {
        private readonly Project _csproj;
        private readonly string _propertyName;
        private readonly string _propertyValue;

        public MissingProperty(Project csproj, string propertyName, string propertyValue)
        {
            _csproj = csproj;
            _propertyName = propertyName;
            _propertyValue = propertyValue;
        }

        public string BuildMessage()
        {
            return $"The project: '{_csproj}' seems to be a library that might be shared. But lack of property '{_propertyName}'. You can possibly set that to: '{_propertyValue}'.";
        }

        public Task TakeActionAsync()
        {
            return _csproj.SetProperty(_propertyName, _propertyValue);
        }
    }
}
