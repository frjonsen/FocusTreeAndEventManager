﻿using FocusTreeManager.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using FocusTreeManager.CodeStructures;

namespace FocusTreeManager.Model
{
    [ProtoContract(AsReferenceDefault = true)]
    public class Focus : ObservableObject
    {
        const string IMAGE_PATH = "/FocusTreeManager;component/GFX/Focus/";

        [ProtoMember(1)]
        private string image;

        public string Image
        {
            get
            {
                return IMAGE_PATH + image + ".png";
            }
            set
            {
                Set<string>(() => this.Image, ref this.image, value);
            }
        }

        public string Icon
        {
            get
            {
                return image;
            }
        }

        [ProtoMember(2)]
        private string uniquename;

        public string UniqueName
        { 
            get
            {
                if (uniquename == null)
                {
                    return "unknown";
                }
                return uniquename;
            }
            set
            {
                Set<string>(() => this.UniqueName, ref this.uniquename, value);
            }
        }

        public string VisibleName
        {
            get
            {
                var locales = (new ViewModelLocator()).Main.Project.getLocalisationWithKey(uniquename);
                string translation = locales != null ? locales.translateKey(uniquename) : null;
                return translation != null ? translation : uniquename;
            }
        }

        public string Description
        {
            get
            {
                var locales = (new ViewModelLocator()).Main.Project.getLocalisationWithKey(uniquename + "_desc");
                string translation = locales != null ? locales.translateKey(uniquename + "_desc") : null;
                return translation != null ? translation : uniquename + "_desc";
            }
        }

        [ProtoMember(3)]
        private int x;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                Set<int>(() => this.X, ref this.x, value);
            }
        }

        [ProtoMember(4)]
        private int y;

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                Set<int>(() => this.Y, ref this.y, value);
            }
        }

        [ProtoMember(7)]
        private int cost;

        public int Cost
        {
            get
            {
                return cost;
            }
            set
            {
                Set<int>(() => this.Cost, ref this.cost, value);
            }
        }

        [ProtoMember(8)]
        private Script internalScript;

        public Script InternalScript
        {
            get
            {
                return internalScript;
            }
            set
            {
                Set<Script>(() => this.InternalScript, ref this.internalScript, value);
            }
        }

        public Point FocusTop { get; set; }

        public Point FocusBottom { get; set; }

        public Point FocusLeft { get; set; }

        public Point FocusRight { get; set; }

        private bool isSelected;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                Set<bool>(() => this.IsSelected, ref this.isSelected, value);
            }
        }

        [ProtoMember(5, AsReference = true)]
        public List<PrerequisitesSet> Prerequisite { get; set; }

        [ProtoMember(6, AsReference = true)]
        public List<MutuallyExclusiveSet> MutualyExclusive { get; set; }

        public RelayCommand EditFocusCommand { get; private set; }

        public RelayCommand DeleteFocusCommand { get; private set; }

        public RelayCommand MutuallyFocusCommand { get; private set; }

        public RelayCommand<string> PrerequisiteFocusCommand { get; private set; }
        
        public RelayCommand TestFinishCommand { get; private set; }

        public Focus()
        {
            MutualyExclusive = new List<MutuallyExclusiveSet>();
            Prerequisite = new List<PrerequisitesSet>();
            EditFocusCommand = new RelayCommand(Edit);
            DeleteFocusCommand = new RelayCommand(Delete);
            MutuallyFocusCommand = new RelayCommand(AddMutuallyExclusive);
            PrerequisiteFocusCommand = new RelayCommand<string>(AddPrerequisite);
            TestFinishCommand = new RelayCommand(FinishSetCommands);
        }

        public void setDefaults()
        {
            InternalScript = new Script();
            Image = "goal_unknown";
            UniqueName = "unknown";
            X = 0;
            Y = 0;
        }

        public void setPoints(Point Top, Point Bottom, Point Left, Point Right)
        {
            FocusTop = Top;
            FocusBottom = Bottom;
            FocusLeft = Left;
            FocusRight = Right;
        }

        public void Edit()
        {
            System.Windows.Application.Current.Properties["Mode"] = "Edit";
            Messenger.Default.Send(new NotificationMessage(this, "ShowEditFocus"));
        }

        public void Delete()
        {
            //Kill the focus sets
            foreach (MutuallyExclusiveSet set in MutualyExclusive.ToList())
            {
                set.DeleteSetRelations();
            }
            foreach (PrerequisitesSet set in Prerequisite.ToList())
            {
                set.DeleteSetRelations();
            }
            Messenger.Default.Send(new NotificationMessage(this, "DeleteFocus"));
        }

        public void AddMutuallyExclusive()
        {
            Messenger.Default.Send(new NotificationMessage(this, "AddFocusMutually"));
        }

        public void FinishSetCommands()
        {
            if ((string)System.Windows.Application.Current.Properties["Mode"] == "Mutually")
            {
                Messenger.Default.Send(new NotificationMessage(this, "FinishAddFocusMutually"));
            }
            if ((string)System.Windows.Application.Current.Properties["Mode"] == "Prerequisite")
            {
                Messenger.Default.Send(new NotificationMessage(this, "FinishAddFocusPrerequisite"));
            }
        }

        public void AddPrerequisite(string Type)
        {
            System.Windows.Application.Current.Properties["ModeParam"] = Type;
            Messenger.Default.Send(new NotificationMessage(this, "AddFocusPrerequisite"));
        }
    }
}
