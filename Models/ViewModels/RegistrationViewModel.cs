using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CCIMS.Web.Models.DTOs;

namespace CCIMS.Web.Models.ViewModels
{
	public class RegistrationViewModel
	{
		public IEnumerable<ProvinceDto> Provinces { get; set; }

	}
}
