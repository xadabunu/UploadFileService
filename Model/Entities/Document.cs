using Model.EnumerationClasses;

namespace Model.Entities;

public class Document
{
    public int Id { get; set; }
    public string TypeCode { get; init; } = string.Empty;
    public string Nom { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string StatutCode { get; set; } = string.Empty;
    public int DemandeId { get; init; }

    public bool IsCorrupted => StatutCode == StatutDocument.Corrompu.Code;
    public bool IsValide => StatutCode == StatutDocument.Valide.Code;
    public bool IsEnCours => StatutCode == StatutDocument.EnCours.Code;
}