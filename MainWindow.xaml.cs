using FatturaWizard.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace FatturaWizard
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<DettaglioLinea> dettaglioLinee;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            // Inizializza la collection per il DataGrid delle linee
            dettaglioLinee = new ObservableCollection<DettaglioLinea>();
            dgDettaglioLinee.ItemsSource = dettaglioLinee;

            // Imposta la data corrente
            dpData.SelectedDate = DateTime.Now;

            // Aggiungi una linea vuota di default
            AggiungiNuovaLinea();
        }

        private void btnAggiungiLinea_Click(object sender, RoutedEventArgs e)
        {
            AggiungiNuovaLinea();
        }

        private void btnRimuoviLinea_Click(object sender, RoutedEventArgs e)
        {
            if (dgDettaglioLinee.SelectedItem is DettaglioLinea selectedItem)
            {
                dettaglioLinee.Remove(selectedItem);
                RicalcolaNumeriLinea();
            }
        }

        private void AggiungiNuovaLinea()
        {
            var nuovaLinea = new DettaglioLinea
            {
                NumeroLinea = dettaglioLinee.Count + 1,
                Quantita = 1,
                AliquotaIVA = 22
            };
            dettaglioLinee.Add(nuovaLinea);
        }

        private void RicalcolaNumeriLinea()
        {
            for (int i = 0; i < dettaglioLinee.Count; i++)
            {
                dettaglioLinee[i].NumeroLinea = i + 1;
            }
        }

        private void btnEsportaXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fattura = CreaFatturaElettronica();
                if (ValidaFattura(fattura))
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "File XML (*.xml)|*.xml|Tutti i file (*.*)|*.*",
                        DefaultExt = ".xml",
                        FileName = $"Fattura_{txtNumero.Text}_{DateTime.Now:yyyyMMdd}.xml"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        EsportaXML(fattura, saveFileDialog.FileName);
                        MessageBox.Show("Fattura esportata correttamente!", "Successo",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'esportazione: {ex.Message}", "Errore",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCaricaXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "File XML (*.xml)|*.xml|Tutti i file (*.*)|*.*",
                    DefaultExt = ".xml"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var fattura = CaricaXML(openFileDialog.FileName);
                    PopolaInterfaccia(fattura);
                    MessageBox.Show("Fattura caricata correttamente!", "Successo",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il caricamento: {ex.Message}", "Errore",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnNuovo_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Tutti i dati non salvati andranno persi. Continuare?",
                                       "Conferma", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                PulisciInterfaccia();
            }
        }

        private FatturaElettronica CreaFatturaElettronica()
        {
            var fattura = new FatturaElettronica
            {
                Versione = "FPR12",
                Header = new FatturaElettronicaHeader
                {
                    DatiTrasmissione = new DatiTrasmissione
                    {
                        IdTrasmittente = new IdTrasmittente
                        {
                            IdPaese = txtIdPaese.Text,
                            IdCodice = txtIdCodice.Text
                        },
                        ProgressivoInvio = txtProgressivoInvio.Text,
                        FormatoTrasmissione = ((ComboBoxItem)cmbFormatoTrasmissione.SelectedItem)?.Content?.ToString(),
                        CodiceDestinatario = txtCodiceDestinatario.Text
                    },
                    CedentePrestatore = new CedentePrestatore
                    {
                        DatiAnagrafici = new DatiAnagrafici
                        {
                            IdFiscaleIVA = new IdFiscaleIVA
                            {
                                IdPaese = txtCedentePaeseIVA.Text,
                                IdCodice = txtCedenteCodiceIVA.Text
                            },
                            CodiceFiscale = txtCedenteCodiceFiscale.Text,
                            Anagrafica = new Anagrafica
                            {
                                Nome = txtCedenteNome.Text,
                                Cognome = txtCedenteCognome.Text,
                                Denominazione = txtCedenteDenominazione.Text
                            }
                        },
                        Sede = new Sede
                        {
                            Indirizzo = txtCedenteIndirizzo.Text,
                            NumeroCivico = txtCedenteNumeroCivico.Text,
                            CAP = txtCedenteCAP.Text,
                            Comune = txtCedenteComune.Text,
                            Provincia = txtCedenteProvincia.Text,
                            Nazione = txtCedenteNazione.Text
                        }
                    },
                    CessionarioCommittente = new CessionarioCommittente
                    {
                        DatiAnagrafici = new DatiAnagrafici
                        {
                            IdFiscaleIVA = new IdFiscaleIVA
                            {
                                IdPaese = txtCessionarioPaeseIVA.Text,
                                IdCodice = txtCessionarioCodiceIVA.Text
                            },
                            CodiceFiscale = txtCessionarioCodiceFiscale.Text,
                            Anagrafica = new Anagrafica
                            {
                                Nome = txtCessionarioNome.Text,
                                Cognome = txtCessionarioCognome.Text,
                                Denominazione = txtCessionarioDenominazione.Text
                            }
                        },
                        Sede = new Sede
                        {
                            Indirizzo = txtCessionarioIndirizzo.Text,
                            NumeroCivico = txtCessionarioNumeroCivico.Text,
                            CAP = txtCessionarioCAP.Text,
                            Comune = txtCessionarioComune.Text,
                            Provincia = txtCessionarioProvincia.Text,
                            Nazione = txtCessionarioNazione.Text
                        }
                    }
                },
                Body = new FatturaElettronicaBody
                {
                    DatiGenerali = new DatiGenerali
                    {
                        DatiGeneraliDocumento = new DatiGeneraliDocumento
                        {
                            TipoDocumento = ((ComboBoxItem)cmbTipoDocumento.SelectedItem)?.Content?.ToString(),
                            Divisa = txtDivisa.Text,
                            Data = dpData.SelectedDate ?? DateTime.Now,
                            Numero = txtNumero.Text,
                            Causale = txtCausale.Text,
                            ImportoTotaleDocumento = ParseDecimal(txtImportoTotale.Text)
                        }
                    },
                    DatiBeniServizi = new DatiBeniServizi
                    {
                        DettaglioLinee = dettaglioLinee.ToList(),
                        DatiRiepilogo = new DatiRiepilogo
                        {
                            AliquotaIVA = ParseDecimal(txtAliquotaIVA.Text),
                            Natura = txtNatura.Text,
                            ImponibileImporto = ParseDecimal(txtImponibile.Text),
                            Imposta = ParseDecimal(txtImposta.Text)
                        }
                    }
                }
            };

            return fattura;
        }

        private decimal ParseDecimal(string value)
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                return result;
            return 0;
        }

        private bool ValidaFattura(FatturaElettronica fattura)
        {
            if (string.IsNullOrEmpty(fattura.Header.DatiTrasmissione.IdTrasmittente.IdCodice))
            {
                MessageBox.Show("Il codice ID del trasmittente è obbligatorio!", "Errore di validazione",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(fattura.Body.DatiGenerali.DatiGeneraliDocumento.Numero))
            {
                MessageBox.Show("Il numero della fattura è obbligatorio!", "Errore di validazione",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (fattura.Body.DatiBeniServizi.DettaglioLinee == null || fattura.Body.DatiBeniServizi.DettaglioLinee.Count == 0)
            {
                MessageBox.Show("È necessario inserire almeno una linea di dettaglio!", "Errore di validazione",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void EsportaXML(FatturaElettronica fattura, string filePath)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2");

            var serializer = new XmlSerializer(typeof(FatturaElettronica));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (var writer = XmlWriter.Create(filePath, settings))
            {
                serializer.Serialize(writer, fattura, namespaces);
            }
        }

        private FatturaElettronica CaricaXML(string filePath)
        {
            var serializer = new XmlSerializer(typeof(FatturaElettronica));
            using (var reader = new FileStream(filePath, FileMode.Open))
            {
                return (FatturaElettronica)serializer.Deserialize(reader);
            }
        }

        private void PopolaInterfaccia(FatturaElettronica fattura)
        {
            // Dati Trasmissione
            txtIdPaese.Text = fattura.Header.DatiTrasmissione.IdTrasmittente.IdPaese;
            txtIdCodice.Text = fattura.Header.DatiTrasmissione.IdTrasmittente.IdCodice;
            txtProgressivoInvio.Text = fattura.Header.DatiTrasmissione.ProgressivoInvio;
            txtCodiceDestinatario.Text = fattura.Header.DatiTrasmissione.CodiceDestinatario;

            // Seleziona il formato trasmissione
            foreach (ComboBoxItem item in cmbFormatoTrasmissione.Items)
            {
                if (item.Content.ToString() == fattura.Header.DatiTrasmissione.FormatoTrasmissione)
                {
                    cmbFormatoTrasmissione.SelectedItem = item;
                    break;
                }
            }

            // Cedente/Prestatore
            txtCedentePaeseIVA.Text = fattura.Header.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA?.IdPaese;
            txtCedenteCodiceIVA.Text = fattura.Header.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA?.IdCodice;
            txtCedenteCodiceFiscale.Text = fattura.Header.CedentePrestatore.DatiAnagrafici.CodiceFiscale;
            txtCedenteNome.Text = fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica?.Nome;
            txtCedenteCognome.Text = fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica?.Cognome;
            txtCedenteDenominazione.Text = fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica?.Denominazione;

            txtCedenteIndirizzo.Text = fattura.Header.CedentePrestatore.Sede?.Indirizzo;
            txtCedenteNumeroCivico.Text = fattura.Header.CedentePrestatore.Sede?.NumeroCivico;
            txtCedenteCAP.Text = fattura.Header.CedentePrestatore.Sede?.CAP;
            txtCedenteComune.Text = fattura.Header.CedentePrestatore.Sede?.Comune;
            txtCedenteProvincia.Text = fattura.Header.CedentePrestatore.Sede?.Provincia;
            txtCedenteNazione.Text = fattura.Header.CedentePrestatore.Sede?.Nazione;

            // Cessionario/Committente
            txtCessionarioPaeseIVA.Text = fattura.Header.CessionarioCommittente.DatiAnagrafici.IdFiscaleIVA?.IdPaese;
            txtCessionarioCodiceIVA.Text = fattura.Header.CessionarioCommittente.DatiAnagrafici.IdFiscaleIVA?.IdCodice;
            txtCessionarioCodiceFiscale.Text = fattura.Header.CessionarioCommittente.DatiAnagrafici.CodiceFiscale;
            txtCessionarioNome.Text = fattura.Header.CessionarioCommittente.DatiAnagrafici.Anagrafica?.Nome;
            txtCessionarioCognome.Text = fattura.Header.CessionarioCommittente.DatiAnagrafici.Anagrafica?.Cognome;
            txtCessionarioDenominazione.Text = fattura.Header.CessionarioCommittente.DatiAnagrafici.Anagrafica?.Denominazione;

            txtCessionarioIndirizzo.Text = fattura.Header.CessionarioCommittente.Sede?.Indirizzo;
            txtCessionarioNumeroCivico.Text = fattura.Header.CessionarioCommittente.Sede?.NumeroCivico;
            txtCessionarioCAP.Text = fattura.Header.CessionarioCommittente.Sede?.CAP;
            txtCessionarioComune.Text = fattura.Header.CessionarioCommittente.Sede?.Comune;
            txtCessionarioProvincia.Text = fattura.Header.CessionarioCommittente.Sede?.Provincia;
            txtCessionarioNazione.Text = fattura.Header.CessionarioCommittente.Sede?.Nazione;

            // Dati Documento
            foreach (ComboBoxItem item in cmbTipoDocumento.Items)
            {
                if (item.Content.ToString() == fattura.Body.DatiGenerali.DatiGeneraliDocumento.TipoDocumento)
                {
                    cmbTipoDocumento.SelectedItem = item;
                    break;
                }
            }

            txtDivisa.Text = fattura.Body.DatiGenerali.DatiGeneraliDocumento.Divisa;
            dpData.SelectedDate = fattura.Body.DatiGenerali.DatiGeneraliDocumento.Data;
            txtNumero.Text = fattura.Body.DatiGenerali.DatiGeneraliDocumento.Numero;
            txtCausale.Text = fattura.Body.DatiGenerali.DatiGeneraliDocumento.Causale;
            txtImportoTotale.Text = fattura.Body.DatiGenerali.DatiGeneraliDocumento.ImportoTotaleDocumento.ToString("F2");

            // Dettaglio Linee
            dettaglioLinee.Clear();
            if (fattura.Body.DatiBeniServizi.DettaglioLinee != null)
            {
                foreach (var linea in fattura.Body.DatiBeniServizi.DettaglioLinee)
                {
                    dettaglioLinee.Add(linea);
                }
            }

            // Dati Riepilogo
            txtAliquotaIVA.Text = fattura.Body.DatiBeniServizi.DatiRiepilogo?.AliquotaIVA.ToString("F2");
            txtNatura.Text = fattura.Body.DatiBeniServizi.DatiRiepilogo?.Natura;
            txtImponibile.Text = fattura.Body.DatiBeniServizi.DatiRiepilogo?.ImponibileImporto.ToString("F2");
            txtImposta.Text = fattura.Body.DatiBeniServizi.DatiRiepilogo?.Imposta.ToString("F2");
        }

        private void PulisciInterfaccia()
        {
            // Resetta tutti i campi di testo
            txtIdPaese.Text = "IT";
            txtIdCodice.Clear();
            txtProgressivoInvio.Clear();
            txtCodiceDestinatario.Clear();

            // Cedente/Prestatore
            txtCedentePaeseIVA.Text = "IT";
            txtCedenteCodiceIVA.Clear();
            txtCedenteCodiceFiscale.Clear();
            txtCedenteNome.Clear();
            txtCedenteCognome.Clear();
            txtCedenteDenominazione.Clear();
            txtCedenteIndirizzo.Clear();
            txtCedenteNumeroCivico.Clear();
            txtCedenteCAP.Clear();
            txtCedenteComune.Clear();
            txtCedenteProvincia.Clear();
            txtCedenteNazione.Text = "IT";

            // Cessionario/Committente
            txtCessionarioPaeseIVA.Text = "IT";
            txtCessionarioCodiceIVA.Clear();
            txtCessionarioCodiceFiscale.Clear();
            txtCessionarioNome.Clear();
            txtCessionarioCognome.Clear();
            txtCessionarioDenominazione.Clear();
            txtCessionarioIndirizzo.Clear();
            txtCessionarioNumeroCivico.Clear();
            txtCessionarioCAP.Clear();
            txtCessionarioComune.Clear();
            txtCessionarioProvincia.Clear();
            txtCessionarioNazione.Text = "IT";

            // Dati Documento
            cmbTipoDocumento.SelectedIndex = 0;
            txtDivisa.Text = "EUR";
            dpData.SelectedDate = DateTime.Now;
            txtNumero.Clear();
            txtCausale.Clear();
            txtImportoTotale.Clear();

            // Dettaglio Linee
            dettaglioLinee.Clear();
            AggiungiNuovaLinea();

            // Dati Riepilogo
            txtAliquotaIVA.Clear();
            txtNatura.Clear();
            txtImponibile.Clear();
            txtImposta.Clear();

            // Formato trasmissione
            cmbFormatoTrasmissione.SelectedIndex = 0;
        }
    }
}