using Assets.Scripts.StatsSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StatsSystem
{
    public interface IStatValueGiver
    {
        float GetStatValue(StatType statType); 
    }
}
