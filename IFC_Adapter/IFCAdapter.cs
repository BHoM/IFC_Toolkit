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

using BH.oM.Adapters.IFC;
using BH.oM.Reflection.Attributes;
using System;
using System.ComponentModel;
using Xbim.Ifc;

namespace BH.Adapter.IFC
{
    public partial class IFCAdapter : BHoMAdapter, IDisposable
    {
        /***************************************************/
        /**** Public properties                         ****/
        /***************************************************/

        [Description("Settings of the IFC adapter.")]
        public IFCSettings IFCSettings { get; set; } = null;


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Initialises the IFC_Adapter with a target location.")]
        [Input("targetLocation", "Path to the IFC file to be interacted with, including file extension.")]
        [Input("settings", "Settings of the IFC adapter.")]
        [Input("active", "If true, the adapter consumes the file at target location and prepares for interacting with it.")]
        public IFCAdapter(string targetLocation, IFCSettings settings = null, bool active = false)
        {
            if (active)
            {
                if (Init(targetLocation))
                    IFCSettings = settings;
            }
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private IfcStore m_LoadedModel;


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/
        
        private bool Init(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                BH.Engine.Reflection.Compute.RecordError("Please specifiy a valid target location.");
                return false;
            }

            if (!location.ToLower().EndsWith(".ifc"))
            {
                BH.Engine.Reflection.Compute.RecordError("The file needs to be in .ifc format.");
                return false;
            }

            try
            {
                m_LoadedModel = IfcStore.Open(location);
            }
            catch (Exception ex)
            {
                BH.Engine.Reflection.Compute.RecordError($"The file failed to load with the following error:\n{ex.Message}");
                return false;
            }
            
            // By default, the objects are appendend to the file if it exists already.
            this.m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateOnly;

            return true;
        }


        /***************************************************/
        /**** IDisposable implementation                ****/
        /***************************************************/

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_LoadedModel.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        /***************************************************/
    }
}

