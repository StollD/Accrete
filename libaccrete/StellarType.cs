using System;
using System.Linq;

namespace Accrete
{
    public class StellarType
    {
        // Fields
        public String star_class;
        public Double temp;
        public String balmer;
        public String lines;
        public Double mass;
        public Double size;
        public Double density;
        public Double lum;
        public Double star_age;

        // Contructor
        public StellarType(String sc, Double t, String b, String l, Double m, Double s, Double d, Double lu, Double a)
        {
            star_class = sc;
            temp = t;
            balmer = b;
            lines = l;
            mass = m;
            size = s;
            density = d;
            lum = lu;
            star_age = a;
        }

        // Defaults
        public static StellarType[] Builtin = new StellarType[]
        {
            new StellarType("O0", 1e10, "weak", "He+ O-II He-II", 40, 17.8, 0.01, 405000, 1e6),
            new StellarType("B0", 30000, "medium", "He", 18, 7.4, 0.1, 13000, 11e6),
            new StellarType("A0", 12000, "strong", "", 3.5, 2.5, 0.3, 80, 440e6),
            new StellarType("F0", 7500, "medium", "", 1.7, 1.4, 1.0, 6.4, 3e9),
            new StellarType("G0", 6000, "weak", "Ca++ Fe++", 1.1, 1.0, 1.4, 1.4, 8e9),
            new StellarType("K0", 5000, "v. weak", "Ca++ Fe++", 0.8, 0.8, 1.8, 0.46, 17e9),
            new StellarType("M0", 3500, "v. weak", "Ca++ TiO2", 0.5, 0.6, 2.5, 0.08, 56e9),
            new StellarType("D0", 1500, "none", "", 0, 0, 2.5, 0.00, 56e9)
        };

        // Accessors
        public static StellarType GetStellarTypeMass(Double mass)
        {
            return Builtin.LastOrDefault(s => mass <= s.mass);
        }

        public static StellarType GetStellarTypeTemp(Double temperature)
        {
            return Builtin.LastOrDefault(s => temperature <= s.temp);
        }
    }
}