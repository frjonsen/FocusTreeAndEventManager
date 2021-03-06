﻿using GalaSoft.MvvmLight;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTreeManager.Model
{
    [ProtoContract]
    public class LocaleContent : ObservableObject
    {
        [ProtoMember(1)]
        private string key;

        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                Set<string>(() => this.Key, ref this.key, value);
            }
        }

        [ProtoMember(2)]
        private string value;

        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                Set<string>(() => this.Value, ref this.value, value);
            }
        }
    }
}
