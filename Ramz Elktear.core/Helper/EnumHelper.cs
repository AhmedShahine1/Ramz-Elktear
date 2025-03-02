﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ramz_Elktear.core.Helper
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? value.ToString();
        }
    }
}
