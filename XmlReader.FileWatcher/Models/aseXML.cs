// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
using System.Collections.Generic;
using System.Xml;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:aseXML:r32")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:aseXML:r32", IsNullable = false)]
public partial class aseXML
{
    private Header headerField;

    private Transactions transactionsField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
    public Header Header
    {
        get
        {
            return headerField;
        }
        set
        {
            headerField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
    public Transactions Transactions
    {
        get
        {
            return this.transactionsField;
        }
        init
        {
            this.transactionsField = value;
        }
    }

    public List<string> CsvIntervalBlockData { get; set; }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Header
{

    private string fromField;

    private string toField;

    private string messageIDField;

    private System.DateTime messageDateField;

    private string transactionGroupField;

    private string priorityField;

    private string marketField;

    /// <remarks/>
    public string From
    {
        get
        {
            return this.fromField;
        }
        set
        {
            this.fromField = value;
        }
    }

    /// <remarks/>
    public string To
    {
        get
        {
            return this.toField;
        }
        set
        {
            this.toField = value;
        }
    }

    /// <remarks/>
    public string MessageID
    {
        get
        {
            return this.messageIDField;
        }
        set
        {
            this.messageIDField = value;
        }
    }

    /// <remarks/>
    public System.DateTime MessageDate
    {
        get
        {
            return this.messageDateField;
        }
        set
        {
            this.messageDateField = value;
        }
    }

    /// <remarks/>
    public string TransactionGroup
    {
        get
        {
            return this.transactionGroupField;
        }
        set
        {
            this.transactionGroupField = value;
        }
    }

    /// <remarks/>
    public string Priority
    {
        get
        {
            return this.priorityField;
        }
        set
        {
            this.priorityField = value;
        }
    }

    /// <remarks/>
    public string Market
    {
        get
        {
            return this.marketField;
        }
        set
        {
            this.marketField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Transactions
{

    private TransactionsTransaction transactionField;

    /// <remarks/>
    public TransactionsTransaction Transaction
    {
        get
        {
            return this.transactionField;
        }
        set
        {
            this.transactionField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class TransactionsTransaction
{

    private TransactionsTransactionMeterDataNotification meterDataNotificationField;

    private System.DateTime transactionDateField;

    private string transactionIDField;

    /// <remarks/>
    public TransactionsTransactionMeterDataNotification MeterDataNotification
    {
        get
        {
            return this.meterDataNotificationField;
        }
        set
        {
            this.meterDataNotificationField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime transactionDate
    {
        get
        {
            return this.transactionDateField;
        }
        set
        {
            this.transactionDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string transactionID
    {
        get
        {
            return this.transactionIDField;
        }
        set
        {
            this.transactionIDField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class TransactionsTransactionMeterDataNotification
{

    private string cSVIntervalDataField;

    private TransactionsTransactionMeterDataNotificationParticipantRole participantRoleField;

    private string versionField;

    /// <remarks/>
    public string CSVIntervalData
    {
        get
        {
            return this.cSVIntervalDataField;
        }
        set
        {
            this.cSVIntervalDataField = value;
        }
    }

    /// <remarks/>
    public TransactionsTransactionMeterDataNotificationParticipantRole ParticipantRole
    {
        get
        {
            return this.participantRoleField;
        }
        set
        {
            this.participantRoleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string version
    {
        get
        {
            return this.versionField;
        }
        set
        {
            this.versionField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class TransactionsTransactionMeterDataNotificationParticipantRole
{

    private string roleField;

    /// <remarks/>
    public string Role
    {
        get
        {
            return this.roleField;
        }
        set
        {
            this.roleField = value;
        }
    }
}

