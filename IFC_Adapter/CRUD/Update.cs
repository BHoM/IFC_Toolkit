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

using BH.Engine.Adapters.IFC;
using BH.Engine.Base;
using BH.oM.Adapter;
using BH.oM.Adapters.IFC;
using BH.oM.Adapters.IFC.Properties;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PropertyResource;

namespace BH.Adapter.IFC
{
    public partial class IfcAdapter : BHoMAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected override bool IUpdate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
        {            
            bool success = true;
            success = Update(objects as dynamic);
            return success;
        }

        /***************************************************/

        protected bool Update(IEnumerable<IBHoMObject> bhomObjects)
        {
            // Get the settings
            IfcSettings settings = this.IFCSettings.DefaultIfNull();
            Dictionary<string, IfcObject> ifcObjects = m_LoadedModel.Instances.OfType<IfcObject>().ToDictionary(x => x.GlobalId.ToString(), x => x);

            foreach (IBHoMObject obj in bhomObjects)
            {
                try
                {
                    string id = obj.FindFragment<IfcIdentifiers>()?.PersistentId as string;
                    if (string.IsNullOrWhiteSpace(id))
                        continue;

                    if (!ifcObjects.ContainsKey(id))
                        continue;

                    ifcObjects[id].CopyIfcPropertiesFromFragment(obj);
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordError($"Update of the IFC element based on BHoM object failed. BHoM_Guid:{obj.BHoM_Guid}\nException message: {e.Message}");
                }
            }

            BH.Engine.Reflection.Compute.RecordWarning("Update of IFC elements is currently limited to updating the existing properties.");
            
            return true;
        }

        /***************************************************/
    }
}


