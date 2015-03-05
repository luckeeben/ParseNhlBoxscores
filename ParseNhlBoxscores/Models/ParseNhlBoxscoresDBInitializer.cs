using System.Data.Entity;

namespace ParseNhlBoxscores.Models
{
    public class ParseNhlBoxscoresDBInitializer : DropCreateDatabaseAlways<ParseNhlBoxscoresContext>
    {
        protected override void Seed(ParseNhlBoxscoresContext context)
        {
            //IList<Standard> defaultStandards = new List<Standard>();

            //defaultStandards.Add(new Standard() { StandardName = "Standard 1", Description = "First Standard" });
            //defaultStandards.Add(new Standard() { StandardName = "Standard 2", Description = "Second Standard" });
            //defaultStandards.Add(new Standard() { StandardName = "Standard 3", Description = "Third Standard" });

            //foreach (Standard std in defaultStandards)
            //    context.Standards.Add(std);

            //base.Seed(context);
        }
    }
}
