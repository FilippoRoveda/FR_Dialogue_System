using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace DS.Editor.Elements
{
    using Editor.Conditions;
    using Editor.Events;

    using Variables.Editor;

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
        public static Port CreatePort( this BaseNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Input, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            return port;
        }
        public static ObjectField AddIntCondition(Conditions container, VisualElement contentContainer, IntCondition condition = null)
        {
            IntCondition _condition;
            if (condition == null)
            {
                _condition = container.AddIntCondition();
            }
            else
            {
                _condition = condition;
            }

            var varField = new ObjectField()
            {
                objectType = typeof(IntegerVariableData),
                allowSceneObjects = true,
                value = _condition.Variable
            };
            varField.RegisterValueChangedCallback(value => { _condition.Variable = (IntegerVariableData)value.newValue; });
            varField.SetValueWithoutNotify(_condition.Variable);
            varField.AddToClassList("SOElement");

            var toolbarMenu = new ToolbarMenu();
            toolbarMenu.text = _condition.ComparisonType.ToString();
            foreach (ComparisonType ComparisonType in (ComparisonType[])System.Enum.GetValues(typeof(ComparisonType)))
            {
                toolbarMenu.menu.AppendAction(ComparisonType.ToString(), callback => {
                    toolbarMenu.text = ComparisonType.ToString();
                    _condition.ComparisonType = ComparisonType;
                });
            }
            toolbarMenu.AddToClassList("ds-enumElement");

            var comparisonValueField = new IntegerField()
            {
                value = _condition.ComparisonValue,
            };
            comparisonValueField.RegisterValueChangedCallback(value =>
            {
                _condition.ComparisonValue = value.newValue;
            });
            comparisonValueField.SetValueWithoutNotify(_condition.ComparisonValue);

            Button deleteConditionButton = ElementsUtilities.CreateButton("X", () => {
                container.RemoveIntCondition(_condition);
                contentContainer.Remove(varField);
            });

            deleteConditionButton.AddToClassList("ds-condition-button");
            comparisonValueField.AddToClassList("ds-intElement");


            varField.Add(toolbarMenu);
            varField.Add(comparisonValueField);
            varField.Add(deleteConditionButton);
            contentContainer.Add(varField);

            return varField;
        }
        public static ObjectField AddFloatCondition(Conditions container, VisualElement contentContainer, FloatCondition condition = null)
        {
            FloatCondition _condition;
            if (condition == null)
            {
                _condition = container.AddFloatCondition();
            }
            else
            {
                _condition = condition;
            }

            var varField = new ObjectField()
            {
                objectType = typeof(FloatVariableData),
                allowSceneObjects = true,
                value = _condition.Variable
            };
            varField.RegisterValueChangedCallback(value => { _condition.Variable = (FloatVariableData)value.newValue; });
            varField.SetValueWithoutNotify(_condition.Variable);
            varField.AddToClassList("SOElement");


            var toolbarMenu = new ToolbarMenu();
            toolbarMenu.text = _condition.ComparisonType.ToString();
            foreach (ComparisonType ComparisonType in (ComparisonType[])System.Enum.GetValues(typeof(ComparisonType)))
            {
                toolbarMenu.menu.AppendAction(ComparisonType.ToString(), callback => {
                    toolbarMenu.text = ComparisonType.ToString();
                    _condition.ComparisonType = ComparisonType;
                });
            }

            var comparisonValueField = new FloatField()
            {
                value = _condition.ComparisonValue,
            };
            comparisonValueField.RegisterValueChangedCallback(value =>
            {
                _condition.ComparisonValue = value.newValue;
            });
            comparisonValueField.SetValueWithoutNotify(_condition.ComparisonValue);

            Button deleteConditionButton = ElementsUtilities.CreateButton("X", () => {
                container.RemoveFloatCondition(_condition);
                contentContainer.Remove(varField);
            });

            deleteConditionButton.AddToClassList("ds-condition-button");
            toolbarMenu.AddToClassList("ds-enumElement");
            comparisonValueField.AddToClassList("ds-intElement");


            varField.Add(toolbarMenu);
            varField.Add(comparisonValueField);
            varField.Add(deleteConditionButton);
            contentContainer.Add(varField);

            return varField;
        }
        public static ObjectField AddBoolCondition(Conditions container, VisualElement contentContainer, BoolCondition condition = null)
        {
            BoolCondition _condition;
            if (condition == null)
            {
                _condition = container.AddBoolCondition();
            }
            else
            {
                _condition = condition;
            }
            var varField = new ObjectField()
            {
                objectType = typeof(BooleanVariableData),
                allowSceneObjects = true,
                value = _condition.Variable
            };
            varField.RegisterValueChangedCallback(value => { _condition.Variable = (BooleanVariableData)value.newValue; });
            varField.SetValueWithoutNotify(_condition.Variable);
            varField.AddToClassList("SOElement");

            var comparisonValueField = new Toggle()
            {
                value = _condition.ComparisonValue,
            };
            comparisonValueField.RegisterValueChangedCallback(value =>
            {
                Debug.Log($"Previews bool val = {_condition.ComparisonValue}");
                _condition.ComparisonValue = value.newValue;
                Debug.Log($"New bool val = {_condition.ComparisonValue}");
            });
            comparisonValueField.SetValueWithoutNotify(_condition.ComparisonValue);

            Button deleteConditionButton = ElementsUtilities.CreateButton("X", () => {
                container.RemoveBoolCondition(_condition);
                contentContainer.Remove(varField);
            });

            deleteConditionButton.AddToClassList("ds-condition-button");
            comparisonValueField.AddToClassList("ds-intElement");

            varField.Add(comparisonValueField);
            varField.Add(deleteConditionButton);
            contentContainer.Add(varField);

            return varField;
        }


        public static ObjectField AddIntVarEvent(VariableEvents container, VisualElement contentContainer, IntegerEvent _event = null)
        {
            IntegerEvent tempEvent;
            if (_event == null)
            {
                tempEvent = container.AddIntEvent();
            }
            else
            {
                tempEvent = _event;
            }

            var varField = new ObjectField()
            {
                objectType = typeof(IntegerVariableData),
                allowSceneObjects = true,
                value = tempEvent.Variable
            };
            varField.RegisterValueChangedCallback(value => { tempEvent.Variable = (IntegerVariableData)value.newValue; });
            varField.SetValueWithoutNotify(tempEvent.Variable);
            varField.AddToClassList("SOElement");

            var toolbarMenu = new ToolbarMenu();
            toolbarMenu.text = tempEvent.EventType.ToString();
            foreach (VariableEventType evenType in (VariableEventType[])System.Enum.GetValues(typeof(VariableEventType)))
            {
                toolbarMenu.menu.AppendAction(evenType.ToString(), callback => {
                    tempEvent.EventType = evenType;
                    toolbarMenu.text = tempEvent.EventType.ToString();
                });
            }
            toolbarMenu.AddToClassList("ds-enumElement");

            var comparisonValueField = new IntegerField()
            {
                value = tempEvent.EventValue,
            };
            comparisonValueField.RegisterValueChangedCallback(value =>
            {
                tempEvent.EventValue = value.newValue;
            });
            comparisonValueField.SetValueWithoutNotify(tempEvent.EventValue);

            Button deleteConditionButton = CreateButton("X", () => {
                container.RemoveIntEvent(tempEvent);
                contentContainer.Remove(varField);
            });

            deleteConditionButton.AddToClassList("ds-condition-button");
            comparisonValueField.AddToClassList("ds-intElement");


            varField.Add(toolbarMenu);
            varField.Add(comparisonValueField);
            varField.Add(deleteConditionButton);
            contentContainer.Add(varField);

            return varField;
        }
        public static ObjectField AddFloatVarEvent(VariableEvents container, VisualElement contentContainer, FloatEvent _event = null)
        {
            FloatEvent tempEvent;
            if (_event == null)
            {
                tempEvent = container.AddFloatEvent();
            }
            else
            {
                tempEvent = _event;
            }

            var varField = new ObjectField()
            {
                objectType = typeof(FloatVariableData),
                allowSceneObjects = true,
                value = tempEvent.Variable
            };
            varField.RegisterValueChangedCallback(value => { tempEvent.Variable = (FloatVariableData)value.newValue; });
            varField.SetValueWithoutNotify(tempEvent.Variable);
            varField.AddToClassList("SOElement");

            var toolbarMenu = new ToolbarMenu();
            toolbarMenu.text = tempEvent.EventType.ToString();
            foreach (VariableEventType evenType in (VariableEventType[])System.Enum.GetValues(typeof(VariableEventType)))
            {
                toolbarMenu.menu.AppendAction(evenType.ToString(), callback => {
                    tempEvent.EventType = evenType;
                    toolbarMenu.text = tempEvent.EventType.ToString();
                });
            }
            toolbarMenu.AddToClassList("ds-enumElement");

            var comparisonValueField = new FloatField()
            {
                value = tempEvent.EventValue,
            };
            comparisonValueField.RegisterValueChangedCallback(value =>
            {
                tempEvent.EventValue = value.newValue;
            });
            comparisonValueField.SetValueWithoutNotify(tempEvent.EventValue);

            Button deleteConditionButton = CreateButton("X", () => {
                container.RemoveFloatEvent(tempEvent);
                contentContainer.Remove(varField);
            });

            deleteConditionButton.AddToClassList("ds-condition-button");
            comparisonValueField.AddToClassList("ds-intElement");


            varField.Add(toolbarMenu);
            varField.Add(comparisonValueField);
            varField.Add(deleteConditionButton);
            contentContainer.Add(varField);

            return varField;
        }
        public static ObjectField AddBoolVarEvent(VariableEvents container, VisualElement contentContainer, BoolEvent _event = null)
        {
            BoolEvent tempEvent;
            if (_event == null)
            {
                tempEvent = container.AddBoolEvent();
            }
            else
            {
                tempEvent = _event;
            }

            var varField = new ObjectField()
            {
                objectType = typeof(BooleanVariableData),
                allowSceneObjects = true,
                value = tempEvent.Variable
            };
            varField.RegisterValueChangedCallback(value => { tempEvent.Variable = (BooleanVariableData)value.newValue; });
            varField.SetValueWithoutNotify(tempEvent.Variable);
            varField.AddToClassList("SOElement");

            var toolbarMenu = new ToolbarMenu();
            toolbarMenu.text = tempEvent.EventType.ToString();
            //foreach (VariableEventType evenType in (VariableEventType[])System.Enum.GetValues(typeof(VariableEventType)))
            //{
            //    toolbarMenu.menu.AppendAction(evenType.ToString(), callback => {
            //        tempEvent.EventType = evenType;
            //        toolbarMenu.text = tempEvent.EventType.ToString();
            //    });
            //}
            toolbarMenu.AddToClassList("ds-enumElement");

            var comparisonValueField = new Toggle()
            {
                value = tempEvent.EventValue,
            };
            comparisonValueField.RegisterValueChangedCallback(value =>
            {
                tempEvent.EventValue = value.newValue;
            });
            comparisonValueField.SetValueWithoutNotify(tempEvent.EventValue);

            Button deleteConditionButton = CreateButton("X", () => {
                container.RemoveBoolEvent(tempEvent);
                contentContainer.Remove(varField);
            });

            deleteConditionButton.AddToClassList("ds-condition-button");
            comparisonValueField.AddToClassList("ds-intElement");


            varField.Add(toolbarMenu);
            varField.Add(comparisonValueField);
            varField.Add(deleteConditionButton);
            contentContainer.Add(varField);

            return varField;
        }

    }
}
