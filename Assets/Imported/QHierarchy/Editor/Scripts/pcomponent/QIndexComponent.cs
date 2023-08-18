using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.phelper;
using qtools.qhierarchy.pdata;

namespace qtools.qhierarchy.pcomponent
{
    public class QIndexComponent : QBaseComponent
    {
        // PRIVATE
        private Color backgroundColor;
        private GUIStyle labelStyle;
        private int index;

        // CONSTRUCTOR
        public QIndexComponent()
        {
            backgroundColor = QResources.getInstance().getColor(QColor.Background);

            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = QResources.getInstance().getColor(QColor.Gray);
            labelStyle.fontSize = 9;
            labelStyle.clipping = TextClipping.Clip;
            labelStyle.alignment = TextAnchor.MiddleRight;

            QSettings.getInstance().addEventListener(QSetting.ShowIndexComponent, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ShowIndexComponentDuringPlayMode, settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled = QSettings.getInstance().get<bool>(QSetting.ShowIndexComponent);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.ShowIndexComponentDuringPlayMode);
        }

        // DRAW
        public override void layout(GameObject gameObject, QObjectList objectList, ref Rect rect)
        {
            rect.x -= 22;
            rect.width = 22;
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect, Rect curRect)
        {
            EditorGUI.DrawRect(curRect, backgroundColor);
            curRect.x -= 2;
            GUI.Label(curRect, gameObject.transform.GetSiblingIndex().ToString(), labelStyle);
        }
    }
}

