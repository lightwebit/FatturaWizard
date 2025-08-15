using System.Xml.Serialization;

namespace FatturaWizard.Models;

[XmlRoot(ElementName = "FatturaElettronica", Namespace = "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2")]
public class FatturaElettronica
{
    [XmlAttribute("versione")]
    public string Versione { get; set; }
    [XmlElement("FatturaElettronicaHeader")]
    public FatturaElettronicaHeader Header { get; set; }
    [XmlElement("FatturaElettronicaBody")]
    public FatturaElettronicaBody Body { get; set; }
}

public class FatturaElettronicaHeader
{
    public DatiTrasmissione DatiTrasmissione { get; set; }
    public CedentePrestatore CedentePrestatore { get; set; }
    public CessionarioCommittente CessionarioCommittente { get; set; }
}

public class DatiTrasmissione
{
    public IdTrasmittente IdTrasmittente { get; set; }
    public string ProgressivoInvio { get; set; }
    public string FormatoTrasmissione { get; set; }
    public string CodiceDestinatario { get; set; }
}

public class IdTrasmittente
{
    public string IdPaese { get; set; }
    public string IdCodice { get; set; }
}

public class CedentePrestatore
{
    public DatiAnagrafici DatiAnagrafici { get; set; }
    public Sede Sede { get; set; }
}

public class CessionarioCommittente
{
    public DatiAnagrafici DatiAnagrafici { get; set; }
    public Sede Sede { get; set; }
}

public class DatiAnagrafici
{
    public IdFiscaleIVA IdFiscaleIVA { get; set; }
    public Anagrafica Anagrafica { get; set; }
    public string CodiceFiscale { get; set; }
    public string RegimeFiscale { get; set; }
}

public class IdFiscaleIVA
{
    public string IdPaese { get; set; }
    public string IdCodice { get; set; }
}

public class Anagrafica
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Denominazione { get; set; }
}

public class Sede
{
    public string Indirizzo { get; set; }
    public string NumeroCivico { get; set; }
    public string CAP { get; set; }
    public string Comune { get; set; }
    public string Provincia { get; set; }
    public string Nazione { get; set; }
}

public class FatturaElettronicaBody
{
    public DatiGenerali DatiGenerali { get; set; }
    public DatiBeniServizi DatiBeniServizi { get; set; }
}

public class DatiGenerali
{
    public DatiGeneraliDocumento DatiGeneraliDocumento { get; set; }
}

public class DatiGeneraliDocumento
{
    public string TipoDocumento { get; set; }
    public string Divisa { get; set; }
    public DateTime Data { get; set; }
    public string Numero { get; set; }
    public string Causale { get; set; }
    public decimal ImportoTotaleDocumento { get; set; }
    public DatiBollo DatiBollo { get; set; }
    public object DatiCassaPrevidenziale { get; set; }
}

public class DatiBollo
{
    public string BolloVirtuale { get; set; }
    public decimal ImportoBollo { get; set; }
}

public class DatiBeniServizi
{
    [XmlElement("DettaglioLinee")]
    public List<DettaglioLinea> DettaglioLinee { get; set; }
    public DatiRiepilogo DatiRiepilogo { get; set; }
}

public class DettaglioLinea
{
    public int NumeroLinea { get; set; }
    public string Descrizione { get; set; }
    public decimal Quantita { get; set; }
    public decimal PrezzoUnitario { get; set; }
    public decimal PrezzoTotale { get; set; }
    public decimal AliquotaIVA { get; set; }
    public string Natura { get; set; }
}

public class DatiRiepilogo
{
    public decimal AliquotaIVA { get; set; }
    public string Natura { get; set; }
    public decimal ImponibileImporto { get; set; }
    public decimal Imposta { get; set; }
}