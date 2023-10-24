using Starfield_DataManagement.Properties;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

using static Starfield_DataManagement.Data.UniverseData;

namespace Starfield_DataManagement.Data
{
    public class DataManager
    {
        private UniverseData Universedata;

        private string SelectedStar;
        private string SelectedPlanet;
        private string SelectedMoon;

        public DataView Stars { get; }
        public DataView Planets { get; }
        public DataView Moons { get; }

        public DataManager()
        {

            Universedata = new UniverseData();

            // TODO: Load Data - data file

            LoadStarData();
            LoadResourceData();

            Stars = Universedata.Star.DefaultView;
            Planets = Universedata.Planet.DefaultView;
            Moons = Universedata.Moon.DefaultView;

            SelectedStar = "";
            SelectedPlanet = "";
            SelectedMoon = "";
        }
            
        private static Stream GetResourceStream(string path)
        {
            Assembly Curr = Assembly.GetExecutingAssembly();

            return Curr.GetManifestResourceStream( Curr.GetManifestResourceNames().Single(str => str.EndsWith(path)));
        }

        private void LoadStarData()
        {
            List<Tuple<string, int>> Stars = new();

            StreamReader StarData = new(GetResourceStream("Stars.txt"));

            if (StarData != null)
            {
                while (!StarData.EndOfStream)
                {
                    string[] Line = StarData.ReadLine().Split('\t');
                    Stars.Add(new(Line[0], Convert.ToInt32(Line[1])));
                }
            }
            List<Tuple<string, string>> Planets = new();

            StreamReader PlanetData = new(GetResourceStream("Planets.txt"));

            while (!PlanetData.EndOfStream)
            {
                string[] Line = PlanetData.ReadLine().Split('\t');
                Planets.Add(new(Line[0], Line[1]));
            }

            List<Tuple<string, string, string>> Moons = new();

            StreamReader MoonData = new(GetResourceStream("Moons.txt"));
            while (!MoonData.EndOfStream)
            {
                string[] Line = MoonData.ReadLine().Split('\t');
                Moons.Add(new(Line[0], Line[1], Line[2]));
            }

            if (Universedata.Star.Count == 0 && Universedata.Planet.Count == 0 && Universedata.Moon.Count == 0 )
            {
                lock (Universedata)
                {
                    foreach (var S in Stars)
                    {
                        StarRow Star = Universedata.Star.FindByStar(S.Item1) ?? Universedata.Star.AddStarRow(S.Item1, S.Item2);

                            foreach (var P in Planets.FindAll((p) => p.Item1 == Star.Star))
                            {
                                PlanetRow Planet = Universedata.Planet.FindByStarPlanet(P.Item1, P.Item2) ?? Universedata.Planet.AddPlanetRow(Star, P.Item2);

                                var M = Moons.FindAll((m) => m.Item1 == Planet.Star && m.Item2 == Planet.Planet);

                                if (M.Count == 0)
                                {
                                    Universedata.Moon.AddMoonRow(Planet.Star, Planet.Planet, "No Moon");
                                }
                                else
                                {
                                    foreach (var moon in M)
                                    {
                                        Universedata.Moon.AddMoonRow(Planet.Star, Planet.Planet, moon.Item3);
                                    }
                                }
                            }
                    }

                    Universedata.Star.AcceptChanges();
                    Universedata.Planet.AcceptChanges();
                    Universedata.Moon.AcceptChanges();
                }
            }
        }

        private void LoadResourceData()
        {
            List<Tuple<string, string, string>> ResourceData = new()
            {
new("Natural","Adhesive","Rare"),
new("Natural","Amino Acids","Rare"),
new("Natural","Analgesic","Rare"),
new("Natural","Antimicrobial","Uncommon"),
new("Natural","Aromatic","Rare"),
new("Natural","Biosuppressant","Exotic"),
new("Natural","Cosmetic","Uncommon"),
new("Natural","Fiber","Common"),
new("Natural","Gastronomic Delight","Unique"),
new("Natural","Hallucinogen","Rare"),
new("Natural","High-Tensile Spidroin","Unique"),
new("Natural","Hypercatalyst","Exotic"),
new("Natural","Immunostimulant","Unique"),
new("Natural","Lubricant","Exotic"),
new("Natural","Luxury Textile","Unique"),
new("Natural","Membrane","Uncommon"),
new("Natural","Memory Substrate","Unique"),
new("Natural","Metabolic Agent","Common"),
new("Natural","Neurologic","Unique"),
new("Natural","Nutrient","Common"),
new("Natural","Ornamental","Uncommon"),
new("Natural","Pigment","Uncommon"),
new("Natural","Polymer","Exotic"),
new("Natural","Quark-Degenerate Tissues","Exotic"),
new("Natural","Sealant","Uncommon"),
new("Natural","Sedative","Rare"),
new("Natural","Solvent","Exotic"),
new("Natural","Spice","Uncommon"),
new("Natural","Stimulant","Exotic"),
new("Natural","Structural","Common"),
new("Natural","Toxin","Common"),
new("Resource","None",""),
new("Resource","Ad-Aldumite",""),
new("Resource","Ag-Silver",""),
new("Resource","Al-Aluminum",""),
new("Resource","Ar-Argon",""),
new("Resource","Au-Gold",""),
new("Resource","Be-Beryllium",""),
new("Resource","C6Hn-Benzene",""),
new("Resource","Cl-Chlorine",""),
new("Resource","Co-Cobalt",""),
new("Resource","Cs-Caesium",""),
new("Resource","Cu-Copper",""),
new("Resource","Dy-Dysprosium",""),
new("Resource","Eu-Europium",""),
new("Resource","F-Fluorine",""),
new("Resource","Fe-Iron",""),
new("Resource","H2O-Water",""),
new("Resource","He-3-Helium",""),
new("Resource","Hg-Mercury",""),
new("Resource","HnCn-Alkanes",""),
new("Resource","Ie-Indicite",""),
new("Resource","IL-Ionic Liquids",""),
new("Resource","Ir-Iridium",""),
new("Resource","Li-Lithium",""),
new("Resource","Nd-Neodymium",""),
new("Resource","Ne-Neon",""),
new("Resource","Ni-Nickel",""),
new("Resource","Pb-Lead",""),
new("Resource","Pd-Palladium",""),
new("Resource","Pt-Platinum",""),
new("Resource","Pu-Plutonium",""),
new("Resource","R-COC-Carboxylic Acids",""),
new("Resource","Rc-Rothicite",""),
new("Resource","Sb-Antimony",""),
new("Resource","SiH3Cl-Chlorosilanes",""),
new("Resource","Ta-Tantalum",""),
new("Resource","Ti-Titanium",""),
new("Resource","Tsn-Tasine",""),
new("Resource","U-Uranium",""),
new("Resource","V-Vanadium",""),
new("Resource","Vr-Veryl",""),
new("Resource","Vy-Vytinium",""),
new("Resource","W-Tungsten",""),
new("Resource","Xe-Xenon",""),
new("Resource","xF4-Tetrafluorides",""),
new("Resource","Yb-Ytterbium",""),
new("Resource","CT-Caelumite","")
            };

            if (Universedata.ResourceData.Count < ResourceData.Count)
            {
                lock (Universedata)
                {
                    foreach (var R in ResourceData)
                    {
                        if (Universedata.ResourceData.FindByResourceTypeResourceName(R.Item1, R.Item2) == null)
                        {
                            Universedata.ResourceData.AddResourceDataRow(R.Item1, R.Item2, R.Item3);
                        }
                    }

                    Universedata.ResourceData.AcceptChanges();
                }
            }
        }

        private DataRow FindRow(DataTable table, string Filter)
        {
            return table.Select(Filter).FirstOrDefault();
        }

        public string[] GetStarList(DataRow[] dataRows)
        {
            return new List<string>(from StarRow sr in dataRows
                           select sr.Star).ToArray();
        }

        public string[] GetPlanetList(DataRow[] dataRows)
        {
            return new List<string>(from PlanetRow pr in dataRows
                                    select pr.Planet).ToArray();
        }

        public string[] GetMoonList(DataRow[] dataRows)
        {
            return new List<string>(from MoonRow mr in dataRows
                                    select mr.Moon).ToArray();
        }

        public DataRow[] SetStarList()
        {
            return (from string s in Settings.Default.SelectedStar
                                      let data = Universedata.Star.Select( $"{Universedata.Star.StarColumn.ColumnName}='{s}'").FirstOrDefault()
                                      where data != null
                                      select data).ToArray();
        }

        public DataRow[] SetPlanetList()
        {
            return (from string s in Settings.Default.SelectedPlanet
                    let data = Universedata.Planet.Select($"{Universedata.Planet.PlanetColumn.ColumnName}='{s}'").FirstOrDefault()
                    where data != null
                    select data).ToArray();

        }

        public DataRow[] SetMoonList()
        {
            return (from string s in Settings.Default.SelectedMoon
                    let data = Universedata.Moon.Select($"{Universedata.Moon.MoonColumn.ColumnName}='{s}'").FirstOrDefault()
                    where data != null
                    select data).ToArray();

        }

    }
}
