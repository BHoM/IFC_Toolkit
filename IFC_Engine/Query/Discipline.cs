/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
 
using BH.oM.Data.Requests;
using BH.oM.Base.Attributes;
using System;
using System.ComponentModel;
using BH.Engine.Data;
using BH.oM.Adapters.IFC;

namespace BH.Engine.Adapters.IFC
{
    public static partial class Query
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        [Description("Gets the discipline to which a given BHoM type belongs. The result is based on the namespace in which the type is declared, e.g. BH.oM.Structure.Elements.Bar will return oM.Adapters.IFC.Discipline.Structural.")]
        [Input("type", "BHoM type to be queried.")]
        [Output("discipline")]
        public static Discipline Discipline(this Type type)
        {
            if (type == null)
                return oM.Adapters.IFC.Discipline.Undefined;

            if (type.Namespace.StartsWith("BH.oM.Structure"))
                return oM.Adapters.IFC.Discipline.Structural;

            if (type.Namespace.StartsWith("BH.oM.Environment"))
                return oM.Adapters.IFC.Discipline.Environmental;

            if (type.Namespace.StartsWith("BH.oM.Architecture"))
                return oM.Adapters.IFC.Discipline.Architecture;

            if (type.Namespace.StartsWith("BH.oM.Physical"))
                return oM.Adapters.IFC.Discipline.Physical;

            if (type.Namespace.StartsWith("BH.oM.Facade"))
                return oM.Adapters.IFC.Discipline.Facade;

            return oM.Adapters.IFC.Discipline.Undefined;
        }

        /***************************************************/

        [Description("Gets discipline enforced by the Request. If the result is different than defaultDiscipline and neither of two is Undefined, null is returned (the result discipline is conflicting with defaultDiscipline).")]
        [Input("request", "BHoM Request to be queried.")]
        [Input("defaultDiscipline", "Default discipline set in adapter's ActionConfig (IFCPullConfig).")]
        [Output("discipline")]
        public static Discipline? Discipline(this IRequest request, Discipline? defaultDiscipline)
        {
            Discipline? discipline = defaultDiscipline;
            if (request is ILogicalRequest)
            {
                foreach (IRequest subRequest in (request as ILogicalRequest).IRequests())
                {
                    discipline = subRequest.Discipline(discipline);
                    if (discipline == null)
                        return null;
                }
            }
            else if (request is FilterRequest)
            {
                Discipline requestDiscipline = (request as FilterRequest).Type.Discipline();

                if (discipline == oM.Adapters.IFC.Discipline.Undefined)
                    discipline = requestDiscipline;
                else if (discipline != requestDiscipline)
                    return null;
            }

            return discipline;
        }

        /***************************************************/
    }
}



