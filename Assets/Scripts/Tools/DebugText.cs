using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utility;

namespace CustomTools
{
    public class DebugText : Singleton<DebugText>
    {
        public new static DebugText Instance => Singleton<DebugText>.Instance;

        private readonly SortedDictionary<string, string> elements = new SortedDictionary<string, string>();
        private readonly StringBuilder sb = new StringBuilder();

        private TMP_Text textBox;


        private void Awake()
        {
            this.AssignGetComponentTo(out textBox);
        }
        
        /// <summary>
        /// Add a value to the debug display
        /// </summary>
        /// <param name="label">The name of the value</param>
        public string this[string label]
        {
            set => elements[label] = value;
        }

        private void Update()
        {
            foreach (var keyValuePair in elements)
            {
                var (label, value) = (keyValuePair.Key, keyValuePair.Value);

                sb.Append($"{label} : {value}{Environment.NewLine}");
            }

            textBox.text = sb.ToString();
            sb.Clear();
        }
    }
}