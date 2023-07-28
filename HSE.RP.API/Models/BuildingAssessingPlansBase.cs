using System;
using HSE.RP.API.Enums;
using HSE.RP.Domain.Entities;

namespace HSE.RP.API.Models
{
	public record BuildingAssessingPlansBase
	{
        public bool? CategoryA { get; set; }
        public bool? CategoryB { get; set; }
        public bool? CategoryC { get; set; }
        public bool? CategoryD { get; set; }
        public bool? CategoryE { get; set; }
        public bool? CategoryF { get; set; }
        public ComponentCompletionState CompletionState { get; set; }
        public List<BuildingInspectorBuildingCategory> MapSelectedCategories()
        {
            //Create empty list
            var selectedCategories = new List<BuildingInspectorBuildingCategory>();
            //Loop through all the categories which are true
            foreach (var category in this.GetType().GetProperties().Where(x => x.PropertyType == typeof(bool) && (bool)x.GetValue(this)))
            {
                //Get the category name
                var categoryName = category.Name;
                //Get the category id
                var categoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName];
                //Add the category to the list
                selectedCategories.Add(new BuildingInspectorBuildingCategory(categoryId, categoryName));
            }
            return selectedCategories;
        }
    }
}

