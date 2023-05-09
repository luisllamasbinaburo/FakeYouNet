using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeYouNet.Classes
{
    public class VoiceCategoryModel
    {
        public string category_token { get; set; }
        public string maybe_super_category_token {get; set;}
        public string model_type {get; set;}

        public string name { get; set; }
        public string name_for_dropdown { get; set; }

        public bool? can_directly_have_models {get; set;}
        public bool? can_have_subcategories {get; set;}
        public bool? can_only_mods_apply {get; set;}

        public bool? is_mod_approved {get; set;}
        public bool? is_synthetic { get; set; }
        public bool? should_be_sorted { get; set; }

        public string created_at {get; set;}
        public string updated_at {get; set;}
        public string deleted_at {get; set;}
    }
}
