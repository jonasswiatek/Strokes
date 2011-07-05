﻿using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using Strokes.BasicAchievements.NRefactory;
using Strokes.Core;

namespace Strokes.BasicAchievements.Achievements
{
    [AchievementDescription("Using statement", AchievementDescription = "Use the using-statement", AchievementCategory = "Basic Achievements")]
    public class UsingStatementAchievement : NRefactoryAchievement
    {
        protected override AbstractAchievementVisitor CreateVisitor()
        {
            return new Visitor();
        }

        private class Visitor : AbstractAchievementVisitor  
        {
            public override object VisitUsingStatement(UsingStatement usingStatement, object data)
            {
                IsAchievementUnlocked = true;
                return base.VisitUsingStatement(usingStatement, data);
            }
        }
    }
}