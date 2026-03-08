using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AroviaApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCommonMedicines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var medicines = new object[,]
            {
                // Analgesics & Antipyretics
                { Guid.NewGuid(), "Paracetamol", "Calpol", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Paracetamol", "Dolo", "650mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Ibuprofen", "Brufen", "400mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Diclofenac", "Voveran", "50mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Aceclofenac", "Zerodol", "100mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Tramadol", "Ultram", "50mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Naproxen", "Naprosyn", "250mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Mefenamic Acid", "Meftal-Spas", "250mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Aspirin", "Ecosprin", "75mg", "Tablet", true, DateTime.UtcNow },

                // Antibiotics
                { Guid.NewGuid(), "Amoxicillin", "Mox", "500mg", "Capsule", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Amoxicillin + Clavulanic Acid", "Augmentin", "625mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Azithromycin", "Azithral", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Cefixime", "Taxim-O", "200mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Ciprofloxacin", "Cifran", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Ofloxacin", "Zenflox", "200mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Doxycycline", "Doxy-1-LDR", "100mg", "Capsule", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Levofloxacin", "Lovo-500", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Clarithromycin", "Claribid", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Metronidazole", "Flagyl", "400mg", "Tablet", true, DateTime.UtcNow },

                // Antihypertensives
                { Guid.NewGuid(), "Amlodipine", "Amlokind", "5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Telmisartan", "Telma", "400mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Losartan", "Losar", "50mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Metoprolol", "Metolar", "25mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Ramipril", "Cardace", "5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Enalapril", "Envas", "5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Hydrochlorothiazide", "Aquazide", "12.5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Bisoprolol", "Concor", "5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Nebivolol", "Nebicard", "5mg", "Tablet", true, DateTime.UtcNow },

                // Antidiabetics
                { Guid.NewGuid(), "Metformin", "Glycomet", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Glimepiride", "Amaryl", "2mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Gliclazide", "Diamicron", "60mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Sitagliptin", "Januvia", "100mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Vildagliptin", "Galvus", "50mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Pioglitazone", "Pioz", "15mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Teneligliptin", "Dynaglipt", "20mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Dapagliflozin", "Forxiga", "10mg", "Tablet", true, DateTime.UtcNow },

                // Gastrointestinal
                { Guid.NewGuid(), "Pantoprazole", "Pan-40", "400mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Omeprazole", "Omez", "20mg", "Capsule", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Rabeprazole", "Rabeloc", "20mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Domperidone", "Domstal", "10mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Ondansetron", "Zofran", "4mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Ranitidine", "Rantac", "150mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Sucralfate", "Sucrafil", "1gm", "Suspension", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Loperamide", "Imodium", "2mg", "Capsule", true, DateTime.UtcNow },

                // Cold & Cough
                { Guid.NewGuid(), "Cetirizine", "Zyrtec", "10mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Levocetirizine", "Levocet", "5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Montelukast", "Singulair", "10mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Fexofenadine", "Allegra", "120mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Chlorpheniramine", "Piriton", "4mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Guaifenesin", "Benadryl", "100mg/5ml", "Syrup", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Dextromethorphan", "Alex", "10mg/5ml", "Syrup", true, DateTime.UtcNow },

                // Vitamins & Supplements
                { Guid.NewGuid(), "Vitamin C", "Limcee", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Vitamin D3", "Shelcal-60k", "60000 IU", "Capsule", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Iron + Folic Acid", "Dexorange", "200ml", "Syrup", true, DateTime.UtcNow },
                { Guid.NewGuid(), "B-Complex", "Becosules", "Capsule", "Capsule", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Calcium + Vitamin D3", "Shelcal-500", "500mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Multivitamin", "Revital", "Capsule", "Capsule", true, DateTime.UtcNow },

                // Cardiovascular
                { Guid.NewGuid(), "Atorvastatin", "Lipitor", "10mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Rosuvastatin", "Rosuvas", "10mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Clopidogrel", "Plavix", "75mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Warfarin", "Coumadin", "5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Digoxin", "Lanoxin", "0.25mg", "Tablet", true, DateTime.UtcNow },

                // Respiratory
                { Guid.NewGuid(), "Salbutamol", "Asthalin", "100mcg", "Inhaler", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Budensonide", "Pulmicort", "200mcg", "Inhaler", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Tiotropium", "Spiriva", "18mcg", "Inhaler", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Ipratropium", "Ipravent", "20mcg", "Inhaler", true, DateTime.UtcNow },

                // Dermatology
                { Guid.NewGuid(), "Clotrimazole", "Candid", "1%", "Cream", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Mupirocin", "T-Bact", "2%", "Ointment", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Betamethasone", "Betnovate", "0.1%", "Cream", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Fusidic Acid", "Fucidin", "2%", "Cream", true, DateTime.UtcNow },

                // Neurological / Psychiatric
                { Guid.NewGuid(), "Alprazolam", "Xanax", "0.25mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Sertraline", "Zoloft", "50mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Amitriptyline", "Tryptomer", "10mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Gabapentin", "Neurontin", "300mg", "Capsule", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Fluoxetine", "Prozac", "20mg", "Capsule", true, DateTime.UtcNow },

                // Hormonal
                { Guid.NewGuid(), "Levothyroxine", "Thyronorm", "50mcg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Prednisolone", "Wysolone", "5mg", "Tablet", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Hydrocortisone", "Wycort", "100mg", "Injection", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Insulin Glargine", "Lantus", "100 units/ml", "Injection", true, DateTime.UtcNow },

                // Eye/Ear Drops
                { Guid.NewGuid(), "Ciprofloxacin Eye Drops", "Ciplox", "5ml", "Drops", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Timolol Eye Drops", "Iotim", "5ml", "Drops", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Wax-Dissolve Ear Drops", "Otorex", "10ml", "Drops", true, DateTime.UtcNow },

                // Miscellaneous
                { Guid.NewGuid(), "Lactulose", "Duphalac", "200ml", "Syrup", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Oral Rehydration Salt", "Electral", "21gm", "Powder", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Multivitamin/Multimineral", "A to Z", "Syrup", "Syrup", true, DateTime.UtcNow },
                { Guid.NewGuid(), "Probiotics", "Vizylac", "Capsule", "Capsule", true, DateTime.UtcNow }
            };

            for (int i = 0; i < medicines.GetLength(0); i++)
            {
                migrationBuilder.InsertData(
                    table: "Medicines",
                    columns: new[] { "Id", "GenericName", "BrandName", "Strength", "Form", "IsActive", "CreatedAt" },
                    values: new object[] { medicines[i, 0], medicines[i, 1], medicines[i, 2], medicines[i, 3], medicines[i, 4], medicines[i, 5], medicines[i, 6] });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
