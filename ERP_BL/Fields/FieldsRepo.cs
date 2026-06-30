using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Fields
{
    public class FieldsRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Getting all Modules by Module
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        //public List<Field> GetAllFieldsByModule(ERP_BL.Enums.TransactionItemType moduleId)
        //{
        //    return context.fields
        //           .Where(x => x.transactionType == moduleId)
        //           .ToList();
        //}

        /// <summary>
        /// Saving fields for a Template
        /// </summary>
        /// <param name="moduleFields"></param>
        public void SaveTemplateFields(ModuleFields moduleFields)
        {
            context.moduleFields.Add(moduleFields);
            context.SaveChanges();
        }

        /// <summary>
        /// Get Template Fields
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ModuleFields GetTemplateFields(int templateId)
        {
            return
            context.moduleFields

            .FirstOrDefault(x => x.templateId == templateId);
        }

        /// <summary>
        /// Get Template Fields
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ModuleFields GetFieldsByTemplateNModule(int templateId, ERP_BL.Enums.TransactionItemType transactionType)
        {
            return
            context.moduleFields

            .FirstOrDefault(x => x.templateId == templateId && x.transactionType == transactionType);
        }


        /// <summary>
        /// Get Template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public Template GetTemplate(int templateId)
        {
            return context.templates
                .FirstOrDefault(x=>x.Id == templateId);
        }

        /// <summary>
        /// Get All templates
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public List<Template> GetAllTemplates()
        {
            return context.templates
                .ToList();
        }

        /// <summary>
        /// Get All templates by module Id
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public List<Template> GetAllTemplatesByModuleId(ERP_BL.Enums.TransactionItemType module)
        {
            return context.templates
                .Where(x => x.transactionType == module)
                .ToList();
        }

        /// <summary>
        /// Add new template
        /// </summary>
        /// <param name="template"></param>
        public void AddTemplate(Template template)
        {
            context.templates.Add(template);
            context.SaveChanges();
        }
        
        /// <summary>
        /// Update existing template
        /// </summary>
        /// <param name="template"></param>
        public void UpdateTemplate(Template template)
        {
            Template templateToUpdate = context.templates.FirstOrDefault(x=>x.Id == template.Id);

            templateToUpdate.Name = template.Name;
            templateToUpdate.transactionType = template.transactionType;
            context.SaveChanges();
        }

        /// <summary>
        /// Add new Module fields
        /// </summary>
        /// <param name="moduleFields"></param>
        public void AddModulesFields(ModuleFields moduleFields)
        {
            context.moduleFields.Add(moduleFields);
            context.SaveChanges();
        }

        /// <summary>
        /// Update module fields
        /// </summary>
        /// <param name="moduleFields"></param>
        public void UpdateModuleFields(ModuleFields moduleFields, List<Field> removeFields)
        {
            ModuleFields _moduleFields = context.moduleFields.FirstOrDefault(x => x.Id == moduleFields.Id);

            _moduleFields = moduleFields;
            if(removeFields.Count > 0)
                context.fields.RemoveRange(removeFields);

            context.SaveChanges();
        }
    }
}
