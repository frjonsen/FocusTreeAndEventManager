﻿using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace FocusTreeManager.Views
{
    /// <summary>
    /// Description for ChangeImage.
    /// </summary>
    public partial class ChangeImage : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the ChangeImage class.
        /// </summary>
        public ChangeImage()
        {
            InitializeComponent();
            loadLocales();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
            this.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.90);
            this.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.90);
        }

        private void NotificationMessageReceived(NotificationMessage msg)
        {
            if (msg.Notification == "HideChangeImage")
            {
                this.Hide();
            }
            if (msg.Notification == "ChangeLanguage")
            {
                loadLocales();
            }
        }

        private void loadLocales()
        {
            ResourceDictionary resourceLocalization = new ResourceDictionary();
            resourceLocalization.Source = new Uri(Configurator.getLanguageFile(), UriKind.Relative);
            this.Resources.MergedDictionaries.Add(resourceLocalization);
        }
    }
}