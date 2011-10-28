﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Strokes.Core;

namespace Strokes.BasicAchievements.Achievements
{
    [AchievementDescriptor("{7BD049ED-8A85-46A7-AE3D-34B0A7EB5B90}", "@DeclareInitializeIntName",
        AchievementDescription = "@DeclareInitializeIntDescription",
        HintUrl = "http://msdn.microsoft.com/en-us/library/ms228360(v=vs.80).aspx",
        AchievementCategory = "@PrimitiveType")]
    public class DeclareInitializeInt : DeclareInitializePrimitiveType<int>
    {
    }
}
