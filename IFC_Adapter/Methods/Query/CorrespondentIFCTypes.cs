/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xbim.Ifc2x3.Interfaces;

namespace BH.Adapter.IFC
{
    public static partial class Query
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        public static IEnumerable<Type> CorrespondentIFCTypes(this Type bHoMType)
        {
            if (bHoMType == null)
                return new List<Type>();

            if (m_IFCTypes.ContainsKey(bHoMType))
                return m_IFCTypes[bHoMType];

            HashSet<Type> iFCTypes = new HashSet<Type>();
            BindingFlags bindingBHoM = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static;
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.IsInterface || !t.IsAbstract || t.Name != "Convert")
                    continue;

                MethodInfo[] typeMethods = t.GetMethods(bindingBHoM);
                Type ienumType = typeof(IEnumerable<>).MakeGenericType(bHoMType);
                foreach (MethodInfo mi in typeMethods.Where(x => x.Name.EndsWith("FromIFC")))
                {
                    if (bHoMType.IsAssignableFrom(mi.ReturnType) || ienumType.IsAssignableFrom(mi.ReturnType))
                    {
                        Type parameterType = mi.GetParameters().First().ParameterType;
                        if (parameterType != typeof(IIfcObject) && typeof(IIfcObject).IsAssignableFrom(parameterType))
                            iFCTypes.Add(parameterType);
                    }
                }
            }

            m_IFCTypes.Add(bHoMType, iFCTypes);
            return iFCTypes;
        }


        /***************************************************/
        /****              Private Fields               ****/
        /***************************************************/

        private static Dictionary<Type, HashSet<Type>> m_IFCTypes = new Dictionary<Type, HashSet<Type>>();

        /***************************************************/
    }
}

