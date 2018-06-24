using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TUI.Flights.Common.ValidationAttributes
{
    public class DepartureDifferentFromDestinationAttribute : Attribute, IModelValidator
    {

        IEnumerable<ModelValidationResult> IModelValidator.Validate(ModelValidationContext context)
        {

            dynamic model = context.Container;

            if (model.DepartureAirportId != 0 && model.DestinationAirportId != 0 && model.DepartureAirportId == model.DestinationAirportId)
            {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("", "You cannot select the same airport as departure and destination in the same time.")
                };

            }

            return System.Linq.Enumerable.Empty<ModelValidationResult>();
        }
    }
}
