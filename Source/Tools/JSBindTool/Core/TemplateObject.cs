using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSBindTool.Core
{
    public enum TemplateType
    {
        SharedPtr=0,
        WeakPtr,
        Vector,
        RefPtr,
        Const
    }
    public class TemplateObject
    {
        public Type TargetType { get; private set; }
        public TemplateType TemplateType { get; private set; }
        public TemplateObject(Type type, TemplateType templateType)
        {
            TargetType = type;
            TemplateType = templateType;
        }

        public static TemplateObject Create(Type type)
        {
            if (!type.IsSubclassOf(typeof(TemplateObject)))
                throw new Exception("invalid template object derived type.");
            var templateObj = Activator.CreateInstance(type) as TemplateObject;
            if (templateObj is null)
                throw new Exception("could not possible to instantiate this template object derived type.");
            return templateObj;
        }
        public static bool IsVectorType(Type type)
        {
            if (!type.IsSubclassOf(typeof(TemplateObject)))
                return false;
            var templateObj = Activator.CreateInstance(type) as TemplateObject;
            return templateObj?.TemplateType == TemplateType.Vector;
        }
    }
}
