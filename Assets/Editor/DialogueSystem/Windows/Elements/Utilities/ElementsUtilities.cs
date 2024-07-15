using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Editor.Windows.Elements
{
    /// <summary>
    /// Utilities class that contain utilities to facilitate Graph ELements.
    /// </summary>
    public static class ElementsUtilities
    {
        /// <summary>
        /// Static method to create a single line TextField.
        /// </summary>
        /// <param name="value">The string value to assign to the field.</param>
        /// <param name="onValueChanged">Callback called on text value changed.</param>
        /// <returns>The generated single line TextField.</returns>
        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };
            if(onValueChanged != null) textField.RegisterValueChangedCallback(onValueChanged);
            
            return textField;
        }

        /// <summary>
        /// Static method to create a multi line TextField.
        /// </summary>
        /// <param name="value">The string value to assign to the field.</param>
        /// <param name="onValueChanged">Callback called on text value changed.</param>
        /// <returns>The generated multi line TextField.</returns>
        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }

        /// <summary>
        /// Static method to create a foldout.
        /// </summary>
        /// <param name="title">The string value to assign to the foldout title.</param>
        /// <param name="isCollapsed">Boolean that decide the default collapsing state of the foldout.</param>
        /// <returns>The generated foldout.</returns>
        public static Foldout CreateFoldout(string title, bool isCollapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !isCollapsed
            };

            return foldout;
        }

        /// <summary>
        /// Static method to create a Button.
        /// </summary>
        /// <param name="text">String to assign to the displayed text on the button.</param>
        /// <param name="onClick">Action called when the button is clicked.</param>
        /// <returns>The generated button.</returns>
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text,
            };
            return button;
        }

        /// <summary>
        /// Static extension method for every DS_Node that create and add a connection Port to the selected Node.
        /// </summary>
        /// <param name="node">The node on which operate.</param>
        /// <param name="portName">The name text displayed on the port.</param>
        /// <param name="orientation">Orientation of the port.</param>
        /// <param name="direction">Direction of the port.</param>
        /// <param name="capacity">Connection capacity of the port.</param>
        /// <returns>The generated port.</returns>
        public static Port CreatePort( this DS_BaseNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Input, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            return port;
        }
    }
}
