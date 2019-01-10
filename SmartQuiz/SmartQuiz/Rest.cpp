#include "stdafx.h"
#include "Rest.h"
#include "WebSocketClient.h"

std::string base_url = "http://localhost:55082/api/";

int Rest::getRRQ(int session)
{
	std::string rrqUrl = base_url + std::to_string(session) + "/getRRQ";
	//std::string rrqUrl = base_url + std::to_string(SESSION) + "/getStream/0";
	std::cout << "RRQ URL IS: " << rrqUrl << std::endl;

	uri* url = new uri(Utilities::convertToWString(rrqUrl).c_str());
	std::string val = std::string(Utilities::HTTPStreamingAsync(url).get());

	std::cout << val << std::endl;

	if (!Utilities::IsJson(val))
		throw std::exception("Call to server unsuccessful!");

	auto j = json::parse(val);

	int rrq_id = int(j["RRQId"]);

	if (rrq_id == 0)
		throw std::exception("Call to server unsuccessful!");

	return int(rrq_id);
}

int Rest::getSessionForRRQ(int rrq_id)
{
	std::string rrqUrl = base_url + "/getSessionForRRQ/" + std::to_string(rrq_id);
	std::cout << "URL IS: " << rrqUrl << std::endl;

	uri* url = new uri(Utilities::convertToWString(rrqUrl).c_str());
	std::string val = std::string(Utilities::HTTPStreamingAsync(url).get());

	if (!Utilities::IsJson(val))
		throw std::exception("Call to server unsuccessful!");

	//auto j = json::parse(val);
	val = val.substr(1, val.length() - 2);

	COutputLogger(val.c_str());

	int session_id = std::stoi(val);
	COutputLogger("SESSION ID Retrieved");
	COutputLogger(std::to_string(session_id).c_str());

	if (session_id == -1)
		COutputLogger("Call to server unsuccessful!");

	return int(session_id);
}

//("api/{sessionId:int}/{rrqId:int}/saveRRQResponse/{QId:int}/{remoteId}/{response}")
bool Rest::postResponse(std::string remoteId, const char * data)
{
	std::string posturl = base_url + std::to_string(SESSION) + "/" + std::to_string(RRQ_ID)+  "/saveRRQResponse/" + std::to_string(Q_ID) + "/" + remoteId + "/" + data;

	uri* url = new uri(Utilities::convertToWString(posturl).c_str());
	std::string val = Utilities::HTTPStreamingAsync(url).get();

	if (val.compare("true")==0)
	{
		COutputLogger("Post Response for Student Call successful");
		return true;
	}
	else
		return false;
}
