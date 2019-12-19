#pragma once

#include "Utilities.h"
#include <nlohmann/json.hpp>
#include "WebSocketClient.h"

namespace Rest
{
	using json = nlohmann::json;
	using namespace web::http;
	int getRRQ(int session);

	int getSessionForRRQ(int rrq_id);

	//("api/{sessionId:int}/{rrqId:int}/saveRRQResponse/{QId:int}/{remoteId}/{response}")
	bool postResponse(std::string remoteId, const char* data);

	std::string getServerUrl(int session);

};