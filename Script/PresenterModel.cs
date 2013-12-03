using System.Collections;
using UnityEngine;

namespace UnitySchool {
	public class PresenterModel {
	
		public PresenterModel() {}
		
		private string sloodleControllerId;
		private string sloodlePwd;
		private string sloodleModuleId;
		private string sloodleSlideNum;		
		private string sloodleSlideUrl;			
		private string sloodleSlideType;
		private string sloodleServerRoot;
		private string presenterLinker;
		private Texture2D textureSlide;
		private bool isLoading;
		
		//GET & SET da variavel sloodleControllerId
		public string SloodleControllerId {
			get { return sloodleControllerId;  }
			set { sloodleControllerId = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public string SloodlePwd {
			get { return sloodlePwd;  }
			set { sloodlePwd = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public string SloodleModuleId {
			get { return sloodleModuleId;  }
			set { sloodleModuleId = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public string SloodleSlideNum {
			get { return sloodleSlideNum;  }
			set { sloodleSlideNum = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public string SloodleSlideUrl {
			get { return sloodleSlideUrl;  }
			set { sloodleSlideUrl = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public string SloodleSlideType {
			get { return sloodleSlideType;  }
			set { sloodleSlideType = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public string SloodleServerRoot {
			get { return sloodleServerRoot;  }
			set { sloodleServerRoot = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public string PresenterLinker {
			get { return presenterLinker;  }
			set { presenterLinker = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public Texture2D TextureSlide {
			get { return textureSlide;  }
			set { textureSlide = value; }
		}
		
		//GET & SET da variavel sloodleControllerId
		public bool IsLoading {
			get { return isLoading;  }
			set { isLoading = value; }
		}
		
	}
}
