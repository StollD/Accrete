using System;

namespace Accrete
{
    /// <summary>
    /// Stores the data for the dust used in the accretation process
    /// </summary>
    public class Dust
    {
        public Double inner_edge;
        public Double outer_edge;
        public Boolean dust_present;
        public Boolean gas_present;
        public Dust next_band;
    }
}