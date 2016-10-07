package com.purlieus.purlieus.fragments;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Spinner;

import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.MobileServiceException;
import com.microsoft.windowsazure.mobileservices.http.ServiceFilterResponse;
import com.microsoft.windowsazure.mobileservices.table.MobileServiceTable;
import com.microsoft.windowsazure.mobileservices.table.TableOperationCallback;
import com.purlieus.purlieus.R;
import com.purlieus.purlieus.adapters.DonorAdapter;
import com.purlieus.purlieus.adapters.SeekAdapter;
import com.purlieus.purlieus.models.BD_Donate;
import com.purlieus.purlieus.models.BD_Seek;

import java.net.MalformedURLException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutionException;

/**
 * Created by anurag on 4/10/16.
 */
public class DonateFragment extends Fragment {

    private Spinner spinner;
    public static final String PROFILE_DATA="profile";
    private MobileServiceClient mClient;
    DonorAdapter donorAdapter;
    Context context;
    List<BD_Donate> donorResult = new ArrayList<BD_Donate>();
    BD_Donate item;
    List<BD_Donate> mList;
    RecyclerView usersRecyclerView;

    public DonateFragment() {
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_donate, container, false);

        spinner = (Spinner)view.findViewById(R.id.bg_donate_spinner);
        String[] groups = {"O+", "O-", "A+", "A-", "B+", "B-", "AB+", "AB-"};
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, groups);
        spinner.setAdapter(adapter);

        item = new BD_Donate();

        try{
            mClient = new MobileServiceClient("https://purlieus.azurewebsites.net", getActivity());
        }
        catch(MalformedURLException e){
            e.printStackTrace();
        }


        SharedPreferences sp = getActivity().getSharedPreferences(PROFILE_DATA, Context.MODE_PRIVATE);
        final SharedPreferences.Editor editor = sp.edit();

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener(){
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                editor.putString("bg_donate", parent.getItemAtPosition(position).toString());
                editor.apply();
                Log.d("Selected", parent.getItemAtPosition(position).toString());
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        /*BD_Donate item = new BD_Donate();
        item.setName(sp.getString("name",""));
        item.setSex(sp.getString("sex",""));
        item.setAge(sp.getInt("age",0));
        item.setBloodGroup(sp.getString("bg",""));
        item.setEmail(sp.getString("email",""));
        item.setContactNumber(sp.getString("contact",""));
        item.setLatitude(sp.getString("latitude",""));
        item.setLongitude(sp.getString("longitude",""));
        item.setPrivate(true);

        mClient.getTable(BD_Donate.class).insert(item);*/

        usersRecyclerView = (RecyclerView)view.findViewById(R.id.donate_recycler_view);
        donorAdapter = new DonorAdapter(getActivity(), donorResult);
        usersRecyclerView.setAdapter(donorAdapter);
        usersRecyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));

        //Donate Button OnClickListener
        final LinearLayout linearLayout = (LinearLayout) view.findViewById(R.id.donate_tab);

        Button donButton = (Button) view.findViewById(R.id.donateButton);
        donButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Log.d("TAG", "Entered On Click");
                usersRecyclerView.setVisibility(View.VISIBLE);
                linearLayout.setVisibility(View.GONE);
            }
        });
        new FetchTask().execute(mClient);


        return view;
    }

    class FetchTask extends AsyncTask<MobileServiceClient, Void, Boolean>{

        @Override
        protected Boolean doInBackground(MobileServiceClient... params) {
            MobileServiceTable<BD_Donate> mTable = params[0].getTable("BD_Donate", BD_Donate.class);
            mList = new ArrayList<>();
            try {
                mList = mTable.execute().get();
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            } catch (MobileServiceException e) {
                e.printStackTrace();
            }

            if (mList.isEmpty())
                Log.d("List", "empty");
            else
                Log.d("Name: ", mList.get(0).getName());

            return true;
        }
        @Override
        protected void onPostExecute(Boolean aBoolean) {
            if (aBoolean) {
                donorResult.clear();
                donorResult.addAll(mList);
                donorAdapter.notifyDataSetChanged();
            }
        }

    }
}
