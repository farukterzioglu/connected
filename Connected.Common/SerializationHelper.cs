#region Header

// /********************************************************
// * <copyright file="SerializationHelper.cs" company="Microsoft">
// * Copyright (C) Microsoft. All rights reserved. *
// * Microsoft Justice & Public Safety Solutions   *
// * jpsinfo@microsoft.com                         *
// * </copyright>
// ********************************************************/
#endregion Header

namespace Connected.Common.SerializationHelper
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// This class has methods for helping in serialization
    /// </summary>
    public static class SerializationHelper
    {
        #region Methods
        ///// <summary>
        ///// This method deserializes the given string to a given type of the object
        ///// </summary>
        ///// <param name="objType">the type of object that the string needs to be deserialized to</param>
        ///// <param name="xmlStringOfObject">the xml representation of the object</param>
        ///// <param name="shouldUseDataContractSerializer">if this is set to true, then a different method will be called</param>
        ///// <returns>the object as represented by the XML</returns>
        //public static object DeserializeUnknownXmlStringToObject(Type objType, string xmlStringOfObject, bool shouldUseDataContractSerializer = false)
        //{
        //    try
        //    {
        //        // create the object ref to dynamically deserialize the string to object
        //        var method = shouldUseDataContractSerializer ?
        //            typeof(SerializationHelper).GetMethod("DataContractSerializerSerializeFromString") :
        //            typeof(SerializationHelper).GetMethod("SerializeFromString");

        //        // make this method generic to accept any "type" of object
        //        var generic = method.MakeGenericMethod(objType);

        //        // create an aarry of parameters to pass to this method
        //        var parametersArray = new object[] { xmlStringOfObject };

        //        // call the method to deserialize teh string to an object
        //        var objectToReturn = generic.Invoke(null, parametersArray);

        //        // return the object
        //        return objectToReturn;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// This method is used to deserialize a string to its object representation
        /// This will be used to in a WF to deserialize the given string to its object form
        /// The method specifies the type "T" that this string should be formatted to
        /// </summary>
        /// <param name="strOfObject">The string that should be converted to its object form</param>
        /// <typeparam name="T">The generic type parameter that the string should be converted to.</typeparam>
        /// <returns>The object representation of the given string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.XML", "CA3070", Justification = "Do not want to use an overload that uses a xml reader.")]
        public static T DeSerializeFromString<T>(string strOfObject)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(strOfObject))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// This method converts the given object to its xml representation
        /// </summary>
        /// <typeparam name="T">The type of object being passed</typeparam>
        /// <param name="objToConvert">the actual object that needs to be converted to xml</param>
        /// <returns>the objects XML representation</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.XML", "CA3056", Justification = "Trusting the xml to be parsed - not coming from a user input.")]
        public static XmlDocument SerializeObjectToXml<T>(object objToConvert)
        {
            // xml settings to control the xml declarations
            var settings = new XmlWriterSettings();

            try
            {
                // to get rid of the indents "\n\r"
                settings.Indent = true;

                // to omit xml declaration
                settings.OmitXmlDeclaration = true;

                // to allow the processing of special characters
                settings.CheckCharacters = false;

                // create the empty namespace object
                var namespaces = new XmlSerializerNamespaces();

                // add empty namespaces
                namespaces.Add(string.Empty, string.Empty);

                // initialize memory stream
                var objMs = new MemoryStream();

                // create the xmlwriter object with the omit xml declaration settings
                var writer = XmlWriter.Create(objMs, settings);

                // convert the object to xml
                var objXMlSerializer = new XmlSerializer(typeof(T));

                // serailze the object to xml, ommiting the namespaces
                objXMlSerializer.Serialize(writer, objToConvert, namespaces);

                // create the xml document object
                var convertedXmlObject = new XmlDocument();

                // reset teh memory stream position to start
                objMs.Position = 0;

                // load the xml into the document
                convertedXmlObject.Load(objMs);

                // return the converted object to the caller
                return convertedXmlObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///// <summary>
        ///// This method clones an object
        ///// </summary>
        ///// <typeparam name="T">the source object to clone</typeparam>
        ///// <param name="source">the object that has been cloned</param>
        ///// <returns>the cloned object</returns>
        //public static T Clone<T>(this T source)
        //{
        //    var obj = source.ToXml();
        //    return (T)obj.ConvertTo<T>();
        //}

        ///// <summary>
        ///// This method is used to compare two objects
        ///// </summary>
        ///// <typeparam name="T">the type of object to compare</typeparam>
        ///// <param name="object1">object one to compare</param>
        ///// <param name="object2">object to be compared against</param>
        ///// <returns>the result of the comparison</returns>
        //public static bool Compare<T>(this T object1, T object2)
        //{
        //    var typeInfo = object1.GetType();
        //    var sourceProperties = typeInfo.GetProperties();

        //    foreach (var pi in sourceProperties.Where(pi => typeInfo.GetProperty(pi.Name).GetValue(object1, null) != null && (typeInfo.GetProperty(pi.Name).GetValue(object2, null) == null)))
        //    {
        //        if ((typeInfo.GetProperty(pi.Name).GetValue(object1, null) == null) || (typeInfo.GetProperty(pi.Name).GetValue(object2, null) == null))
        //        {
        //            return false;
        //        }

        //        if (typeInfo.GetProperty(pi.Name).GetValue(object1, null).ToString() != typeInfo.GetProperty(pi.Name).GetValue(object2, null).ToString())
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// This deserializes an xml string to its object representation
        ///// </summary>
        ///// <typeparam name="T">the type to convert the xml string to its object representation</typeparam>
        ///// <param name="xmlString">the input xml string</param>
        ///// <returns>the deserialized object</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Xml", "CA3070", Justification = "Do not want to use an overload that uses a xml reader.")]
        //public static T ConvertTo<T>(this string xmlString)
        //{
        //    var xs = new XmlSerializer(typeof(T));
        //    var memoryStream = new MemoryStream(StringToByteArray(xmlString));
        //    var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //    return (T)xs.Deserialize(memoryStream);
        //}

        ///// <summary>
        ///// This method is used to deserialize a string to its object representation
        ///// This will be used to in a WF to deserialize the given string to its object form
        ///// The method specifies the type "T" that this string should be formatted to
        ///// </summary>
        ///// <param name="strOfObject">The string that should be converted to its object form</param>
        ///// <typeparam name="T">The generic type parameter that the string should be converted to.</typeparam>
        ///// <returns>The object representation of the given string</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Xml", "CA3054", Justification = "Trusting the xml to be parsed - not coming from a user input.")]
        //public static T DataContractSerializerSerializeFromString<T>(string strOfObject)
        //{
        //    using (var reader = new XmlTextReader(new StringReader(strOfObject)))
        //    {
        //        var deserializer = new DataContractSerializer(typeof(T), null, int.MaxValue, false, true, null);
        //        return (T)deserializer.ReadObject(reader);
        //    }
        //}

        ///// <summary>
        ///// This method is used to deserialize a string to its object representation
        ///// This will be used to in a WF to deserialize the given string to its object form
        ///// The method specifies the type "T" that this string should be formatted to
        ///// </summary>
        ///// <param name="objToBeDeserialized">The object that should be converted to its XML form</param>
        ///// <typeparam name="T">The generic type parameter that the object should be converted from.</typeparam>
        ///// <returns>The xml representation of the given object</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Xml", "CA3057", Justification = "Trusting the xml to be parsed - not coming from a user input.")]
        //public static XmlDocument DataContractSerializerToSerializeToXml<T>(T objToBeDeserialized)
        //{
        //    var xml = new StringBuilder();
        //    using (var writer = new XmlTextWriter(new StringWriter(xml, CultureInfo.InvariantCulture)))
        //    {
        //        var serializer = new DataContractSerializer(objToBeDeserialized.GetType(), null, int.MaxValue, false, true, null);
        //        serializer.WriteObject(writer, objToBeDeserialized);
        //    }

        //    var xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(xml.ToString());
        //    return xmlDoc;
        //}
    
        ///// <summary>
        ///// this method converts the given xml to its object representation
        ///// </summary>
        ///// <typeparam name="T">the type of object represented by the XML</typeparam>
        ///// <param name="xmlOfObject">the xml string that needs to be converted to its object representation</param>
        ///// <returns>the object representation of the given xml</returns>
        //public static T DeserializeXmlStringToObject<T>(string xmlOfObject)
        //{
        //    try
        //    {
        //        // Initializing the XmlSerializer object to type of Error
        //        var objXmlSerializer = new XmlSerializer(typeof(T));

        //        // Reading the contents of Canonical object Body into an xmlReader
        //        var xmlReader = XmlReader.Create(new System.IO.StringReader(xmlOfObject));

        //        if (objXmlSerializer.CanDeserialize(xmlReader))
        //        {
        //            // Deserializing the error object from Canonical object
        //            object objToReturn = (T)objXmlSerializer.Deserialize(xmlReader);

        //            // return converted object
        //            return (T)objToReturn;
        //        }
        //        else
        //        {
        //            throw new System.Exception(string.Format(CultureInfo.InvariantCulture, "Cannot deserialize the given xml to its object representation. The given XML is: \n{0}", xmlOfObject));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// this method converts the given xml to its object representation
        ///// </summary>
        ///// <typeparam name="T">the type of object represented by the XML</typeparam>
        ///// <param name="xmlOfObject">the xml that needs to be converted to its object representation</param>
        ///// <returns>the object representation of the given xml</returns>
        //public static T DeserializeXmlToObject<T>(XmlDocument xmlOfObject)
        //{
        //    try
        //    {
        //        // Initializing the XmlSerializer object to type of Error
        //        var objXmlSerializer = new XmlSerializer(typeof(T));

        //        // Reading the contents of Canonical object Body into an xmlReader
        //        var xmlReader = XmlReader.Create(new System.IO.StringReader(xmlOfObject.OuterXml));

        //        if (objXmlSerializer.CanDeserialize(xmlReader))
        //        {
        //            // Deserializing the error object from Canonical object
        //            object objToReturn = (T)objXmlSerializer.Deserialize(xmlReader);

        //            // return converted object
        //            return (T)objToReturn;
        //        }
        //        else
        //        {
        //            throw new System.Exception(string.Format(CultureInfo.InvariantCulture, "Cannot deserialize the given xml to its object representation. The given XML is: \n{0}", xmlOfObject));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        
        ///// <summary>
        ///// This method is used to serialize an object to its string representation
        ///// This will be used to serialize an object as string and pass it is a string
        ///// to a WF for processing
        ///// </summary>
        ///// <param name="obj">The object that should be converted to a string</param>
        ///// <returns>The string representation of the object</returns>
        ///// [Obsolete("This method is now Obsolete and will be replaced by Microsoft.JPS.Integration.Common", true)]
        //public static string SerializeToString(object obj)
        //{
        //    var serializer = new XmlSerializer(obj.GetType());

        //    using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
        //    {
        //        serializer.Serialize(writer, obj);

        //        return writer.ToString();
        //    }
        //}

        ///// <summary>
        ///// This is the extension method to serialize an object to its xml representation
        ///// </summary>
        ///// <param name="obj">the object to serialize</param>
        ///// <returns>the string xml representation of the object</returns>
        //public static string ToXml(this object obj)
        //{
        //    if (obj == null)
        //    {
        //        throw new ArgumentNullException("obj");
        //    }

        //    return ToXml(obj, false, true);
        //}

        ///// <summary>
        ///// This method converts the byte array to string
        ///// </summary>
        ///// <param name="characters">the character of bytes</param>
        ///// <returns>string representation of the character array</returns>
        //private static string ByteArrayToString(byte[] characters)
        //{
        //    if (characters == null)
        //    {
        //        throw new ArgumentNullException("characters");
        //    }

        //    var constructedString = Encoding.UTF8.GetString(characters);

        //    return constructedString;
        //}

        ///// <summary>
        ///// The method converts a string to byte array
        ///// </summary>
        ///// <param name="xmlString">input string to convert to a byte array</param>
        ///// <returns>the byte array that represents the string</returns>
        //private static byte[] StringToByteArray(string xmlString)
        //{
        //    if (xmlString == null)
        //    {
        //        throw new ArgumentNullException("xmlString");
        //    }

        //    var byteArray = Encoding.UTF8.GetBytes(xmlString);
        //    return byteArray;
        //}

        ///// <summary>
        ///// The extension method to convert any object to an XML representation
        ///// </summary>
        ///// <param name="obj">the object that needs to be serialized to an xml</param>
        ///// <param name="replaceNewLineChar">If true, then replace new line char; if false, then don't replace </param>
        ///// <param name="returnEmptyIfNullOrError">if true, it will return empty xml, otherwise error</param>
        ///// <returns>the xml representation of the object</returns>
        //private static string ToXml(this object obj, bool replaceNewLineChar, bool returnEmptyIfNullOrError)
        //{
        //    try
        //    {
        //        const string Newlinechar = "\r\n";
        //        if (obj != null)
        //        {
        //            string xmlizedString = null;
        //            var memoryStream = new MemoryStream();
        //            var xs = new XmlSerializer(obj.GetType());
        //            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

        //            xs.Serialize(xmlTextWriter, obj);
        //            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        //            xmlizedString = ByteArrayToString(memoryStream.ToArray());
        //            if (replaceNewLineChar)
        //            {
        //                return xmlizedString.ToString().Replace(Newlinechar, string.Empty);
        //            }
        //            else
        //            {
        //                return xmlizedString.ToString();
        //            }
        //        }
        //        else
        //        {
        //            return returnEmptyIfNullOrError ? string.Empty : null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return returnEmptyIfNullOrError ? ex.Message : null;
        //    }
        //}

        ///// <summary>
        ///// This method is the actual method that adds an extension method to objects
        ///// </summary>
        ///// <param name="obj">the object to add the extension method to</param>
        ///// <param name="replaceNewLineChar">value indicating if new line chars should be replaced or not</param>
        ///// <returns>the serialized xml representation of the object</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Avoid uncalled private code")]
        //private static string ToXml(this object obj, bool replaceNewLineChar)
        //{
        //    return ToXml(obj, true, true);
        //}

        ///// <summary>
        ///// This method is the actual method that adds an extension method to objects
        ///// </summary>
        ///// <param name="instance">the instance of the object to add the extension method to</param>
        ///// <param name="ns">the namespace parameter</param>
        ///// <returns>the serialized xml representation of the object</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Avoid uncalled private code")]
        //private static string ToXml(this object instance, XmlSerializerNamespaces ns)
        //{
        //    if (instance != null)
        //    {
        //        var serializer = new XmlSerializer(instance.GetType());
        //        var writer = new StringWriter(CultureInfo.InvariantCulture);
        //        serializer.Serialize(writer, instance, ns);
        //        return writer.ToString();
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        #endregion Methods
    }
}