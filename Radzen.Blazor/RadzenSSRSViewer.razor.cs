﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Radzen.Blazor
{
    public partial class RadzenSSRSViewer : RadzenComponent
    {
        [Parameter]
        public bool UseProxy { get; set; } = false;

        [Parameter]
        public string ReportServer { get; set; }

        [Parameter]
        public string LocalServer { get; set; }

        [Parameter]
        public string ReportName { get; set; }

        [Parameter]
        public RenderFragment Parameters { get; set; }

        public string ReportUrl
        {
            get
            {
                var urlParams = string.Join("&", parameters.Where(p => !string.IsNullOrEmpty(p.ParameterName)).Select(p => $"{p.ParameterName}={p.Value}"));
                var urlParamString = parameters.Count > 0 ? $"&{urlParams}" : urlParams;

                var url = $"{ReportServer}/Pages/ReportViewer.aspx?%2f{ReportName}&rs:Command=Render&rs:Embed=true{urlParamString}";

                if (UseProxy)
                {
                    if (!string.IsNullOrEmpty(LocalServer))
                    {
                        url = $"{LocalServer}/__ssrsreport?url={System.Net.WebUtility.UrlEncode(url)}";
                    }
                    else
                    {
                        url = $"{uriHelper.BaseUri}__ssrsreport?url={System.Net.WebUtility.UrlEncode(url)}";
                    }
                }

                return url;
            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        List<RadzenSSRSViewerParameter> parameters = new List<RadzenSSRSViewerParameter>();

        public void AddParameter(RadzenSSRSViewerParameter parameter)
        {
            if (parameters.IndexOf(parameter) == -1)
            {
                parameters.Add(parameter);
                StateHasChanged();
            }
        }

        protected override string GetComponentCssClass()
        {
            return "ssrsviewer";
        }
    }
}