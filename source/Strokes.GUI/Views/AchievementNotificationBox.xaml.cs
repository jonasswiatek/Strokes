﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Strokes.Core;
using Strokes.Core.Data.Model;
using System.ComponentModel;
using System.Windows.Shapes;

namespace Strokes.GUI.Views
{
    /// <summary>
    /// Interaction logic for AchievementNotificationBox.xaml
    /// </summary>
    public partial class AchievementNotificationBox : Window
    {
        private bool isEventsBound;

        public AchievementNotificationBox()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this) == false)
                UnlockedAchievementsList.LayoutUpdated += UnlockedAchievementsList_LayoutUpdated;

            // Closes the window again if another detection session is launched by the Achievement context.
            AchievementContext.AchievementDetectionStarting += (sender, args) => Close();
        }

        private AchievementNotificationViewModel ViewModel
        {
            get
            {
                return DataContext as AchievementNotificationViewModel;
            }
        }

        /// <summary>
        /// Locates the magnifying glass inside the templates, and binds an event to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnlockedAchievementsList_LayoutUpdated(object sender, EventArgs e)
        {
            if (isEventsBound)
                return;

            isEventsBound = true;

            foreach (Achievement item in UnlockedAchievementsList.Items)
            {
                var dataItem = item;
                var gotoCodebutton = FindItemControl(UnlockedAchievementsList, "PART_MagnifierGlassPath", item) as FrameworkElement;

                if (gotoCodebutton == null)
                {
                    continue;
                }

                if (dataItem.CodeLocation == null)
                {
                    gotoCodebutton.IsEnabled = false;
                }
                else
                {
                    gotoCodebutton.MouseDown += (se, args) =>
                    {
                        var achevementDescriptor = dataItem;

                        AchievementContext.OnAchievementClicked(gotoCodebutton, new AchievementClickedEventArgs
                        {
                            AchievementDescriptor = achevementDescriptor,
                            UIElement = new AchievementViewportControl()
                            {
                                DataContext = achevementDescriptor
                            }
                        });
                    };
                }
            }
        }

        private static object FindItemControl(ItemsControl itemsControl, string controlName, object item)
        {
            var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
            container.ApplyTemplate();

            return container.ContentTemplate.FindName(controlName, container);
        }

        private void CloseWindowImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }

        protected void AddAchievements(IEnumerable<Achievement> achievementDescriptors)
        {
            foreach (var achevementDescriptor in achievementDescriptors)
            {
                ViewModel.CurrentAchievements.Add(achevementDescriptor);
            }
        }

        public static void ShowAchievements(IEnumerable<Achievement> achievementDescriptors)
        {
            if (achievementDescriptors.Any() == false)
            {
                return;
            }

            var instance = new AchievementNotificationBox();
            instance.AddAchievements(achievementDescriptors);

            // This is only to support the Strokes.Console-project
            if (Application.Current != null)
            {
                // During a real Visual Studio integrated run, this is called.
                instance.Show();

                const int rightMargin = 1;
                const int bottomMargin = 8;

                if (Application.Current.MainWindow.WindowState == WindowState.Normal &&
                    Application.Current.MainWindow != instance)
                {
                    instance.Left = Application.Current.MainWindow.Left +
                                    Application.Current.MainWindow.Width - instance.Width - rightMargin;

                    instance.Top = Application.Current.MainWindow.Top +
                                   Application.Current.MainWindow.Height - instance.Height - bottomMargin;
                }
                else
                {
                    instance.Left = SystemParameters.PrimaryScreenWidth - instance.Width - rightMargin;
                    instance.Top = SystemParameters.MaximizedPrimaryScreenHeight -
                                   SystemParameters.ResizeFrameHorizontalBorderHeight - instance.Height - bottomMargin;
                }
            }
            else
            {
                // When activated from a console-app, this is called.
                new Application().Run(instance);
            }
        }
    }
}