using Lab31.App.Domain;
using Lab31.App.Domain.Skills;

namespace Lab31.App.Metadata;

// Дескриптор для типу "McdonaldsWorker"
public sealed class McdonaldsWorkerDescriptor : PersonDescriptor<McdonaldsWorker>
{
    private readonly AttributeSpec[] _attributes;

    public McdonaldsWorkerDescriptor()
    {
        _attributes = new[]
        {
            FirstNameAttribute(),
            LastNameAttribute(),
            Attr("employeeId", "Табельний номер", Patterns.WorkerEmployeeId, Patterns.WorkerEmployeeIdHint, true, string.Empty, w => w.EmployeeId),
            Attr("restaurant", "Ресторан (необов'язково)", Patterns.FreeText, Patterns.FreeTextHint, false, McdonaldsWorker.DefaultRestaurant, w => w.Restaurant)
        };
    }

    public override string TypeName => "McdonaldsWorker";

    public override AttributeSpec[] Attributes => _attributes;

    protected override McdonaldsWorker CreateTyped(string[] values) => new McdonaldsWorker(
        values[0], // firstName
        values[1], // lastName
        values[2], // employeeId
        values[3], // restaurant
        new ISkill[] { new StudySkill("стажування"), new ChessSkill() }); // Навички працівника
}