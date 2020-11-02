using Sequel.Demo.Business.Models;
using Sequel.Demo.Business.Repositories;
using Sequel.Infrastructure.Operations;
using Sequel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequel.Demo.Business.Commands
{
    public class ThingHaveNameRule : IValidationRule<CreateThingWithOtherStuffsCommandArgs>
    {
        public string Message => "Things must have a name";

        public bool Validate(CreateThingWithOtherStuffsCommandArgs entity)
        {
            return entity.Name != string.Empty;
        }
    }

    public class CreateThingWithOtherStuffsCommandArgs
    {
        public string Name { get; set; }
        public CreateOtherStuffArgs[] OtherStuffs { get; set; }
    }

    public class CreateOtherStuffArgs
    {
        public int Number { get; set; }
    }

    public class CreateThingWithOtherStuffsCommand : BaseCommand<CreateThingWithOtherStuffsCommandArgs, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IThingRepository _thingRepository;

        public CreateThingWithOtherStuffsCommand(IUnitOfWork unitOfWork, IThingRepository thingRepository)
        {
            _unitOfWork = unitOfWork;
            _thingRepository = thingRepository;
        }

        public override ValidationResult CanExecute(CreateThingWithOtherStuffsCommandArgs args)
        {
            return Validator.Must<ThingHaveNameRule>().GetValidationResult(args);
        }

        public override OperationResult<Guid> Execute(CreateThingWithOtherStuffsCommandArgs args)
        {
            using (_unitOfWork)
            {
                Thing newThing = new Thing()
                {
                    Id = Guid.NewGuid(),
                    Name = args.Name,
                    OtherStuffs = args.OtherStuffs.Select(o => new OtherStuff() { Id = Guid.NewGuid(), Number = o.Number }).ToList()
                };
                _thingRepository.Insert(newThing);
                _unitOfWork.SaveChanges();

                return new OperationResult<Guid>(OperationStatus.Success, newThing.Id);
            }

        }
    }
}
