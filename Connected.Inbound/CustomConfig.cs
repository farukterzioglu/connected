using System;
using System.Collections.Generic;
using System.Configuration;

//Absolute
namespace __Router
{
//<configSections>
//    <section name="messageTypes" type="Router.MessageTypesSection, Router" allowLocation="true" allowDefinition="Everywhere" />
//    <section name="receiveAdapters" type="Router.ReceiveAdaptersSection, Router" allowLocation="true" allowDefinition="Everywhere" />
//    <section name="registrations" type="Router.RegistrationsSection, Router" allowLocation="true" allowDefinition="Everywhere" />
//    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
//    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
//  </configSections>
//  <!-- Section group -->
//  <messageTypes>
//    <add typeName="counterData" schemaPath="" />
//    <add typeName="testData" schemaPath="" />
//  </messageTypes>
//  <receiveAdapters>
//    <add receiveAdapterName="ekinCounterReceiveAdapter" url="http://localhost:8189/ReceiveAdapter" />
//  </receiveAdapters>
//  <registrations>
//    <add name="counterData2EkinCounter" messageType="counterData" receiveAdapterName="ekinCounterReceiveAdapter" />
//    <add name="testData2EkinCounter" messageType="testData" receiveAdapterName="ekinCounterReceiveAdapter" />
//  </registrations>
//  <!-- Section group -->

    #region Config Classes
    //Message Types Section
    public class MessageTypesSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public MessageTypeCollection Instances
        {
            get { return (MessageTypeCollection)this[""]; }
            set { this[""] = value; }
        }
    }
    public class MessageTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MessageTypeConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MessageTypeConfig)element).TypeName;
        }
    }
    public class MessageTypeConfig : ConfigurationElement
    {
        [ConfigurationProperty("typeName", IsKey = true, IsRequired = true)] //DefaultValue = "DefaultType",
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String TypeName
        {
            get
            {
                return (String)this["typeName"];
            }
            set
            {
                this["typeName"] = value;
            }
        }

        [ConfigurationProperty("schemaPath", IsRequired = false)] //DefaultValue="DefaultSchemaPath", 
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{};'\"|", MinLength = 1, MaxLength = 60)]
        public String SchemaPath
        {
            get
            {
                return (String)this["schemaPath"];
            }
            set
            {
                this["schemaPath"] = value;
            }
        }
    }

    //Receive Adapters Section
    public class ReceiveAdaptersSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ReceiveAdapterCollection Instances
        {
            get { return (ReceiveAdapterCollection)this[""]; }
            set { this[""] = value; }
        }
    }
    public class ReceiveAdapterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ReceiveAdapterConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReceiveAdapterConfig)element).ReceiveAdapterName;
        }
    }
    public class ReceiveAdapterConfig : ConfigurationElement
    {
        [ConfigurationProperty("receiveAdapterName", IsRequired = true)]
        public String ReceiveAdapterName
        {
            get
            {
                return (String)this["receiveAdapterName"];
            }
            set
            {
                this["receiveAdapterName"] = value;
            }
        }

        [ConfigurationProperty("url", IsRequired = false)]
        public String URL
        {
            get
            {
                return (String)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }
    }

    //Registrations Section
    public class RegistrationsSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public RegistrationCollection Instances
        {
            get { return (RegistrationCollection)this[""]; }
            set { this[""] = value; }
        }
    }
    public class RegistrationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RegistrationConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RegistrationConfig)element).Name;
        }
    }
    public class RegistrationConfig : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("messageType", IsRequired = true)]
        public String MessageType
        {
            get
            {
                return (String)this["messageType"];
            }
            set
            {
                this["messageType"] = value;
            }
        }

        [ConfigurationProperty("receiveAdapterName", IsRequired = true)]
        public String ReceiveAdapterName
        {
            get
            {
                return (String)this["receiveAdapterName"];
            }
            set
            {
                this["receiveAdapterName"] = value;
            }
        }
    }
    #endregion  
}
