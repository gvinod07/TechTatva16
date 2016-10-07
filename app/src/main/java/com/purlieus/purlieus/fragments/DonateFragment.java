package com.purlieus.purlieus.fragments;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.location.Location;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.SwitchCompat;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.LinearLayout;
import android.widget.Spinner;
import android.widget.Switch;

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
import java.util.LinkedList;
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
    List<BD_Seek> donorResult = new ArrayList<BD_Seek>();
    List<BD_Seek> mList;
    RecyclerView usersRecyclerView;
    SharedPreferences sp;
    SwitchCompat switchCompat;
    private BD_Donate donor;
    ProgressDialog progressDialog;

    LinearLayout emptyListCondition;
    LinearLayout fullListCondition;

    public DonateFragment() {
    }

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        progressDialog = new ProgressDialog(context);

        sp = getActivity().getSharedPreferences(PROFILE_DATA, Context.MODE_PRIVATE);
        donor = new BD_Donate();
        donor.setName(sp.getString("name",""));
        donor.setSex(sp.getString("sex",""));
        donor.setAge(sp.getInt("age",0));
        donor.setEmail(sp.getString("email",""));
        donor.setContactNumber(sp.getString("contact",""));
        donor.setLatitude(sp.getString("latitude",""));
        donor.setLongitude(sp.getString("longitude",""));
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_donate, container, false);

        emptyListCondition = (LinearLayout) view.findViewById(R.id.no_donors_layout);
        fullListCondition = (LinearLayout) view.findViewById(R.id.donate_tab);

        spinner = (Spinner)view.findViewById(R.id.bg_donate_spinner);
        switchCompat = (SwitchCompat) view.findViewById(R.id.donor_private_switch);

        String[] groups = {"O+", "O-", "A+", "A-", "B+", "B-", "AB+", "AB-"};
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, groups);
        spinner.setAdapter(adapter);

        try{
            mClient = new MobileServiceClient("https://purlieus.azurewebsites.net", getActivity());
        }
        catch(MalformedURLException e){
            e.printStackTrace();
        }
        
        final SharedPreferences.Editor editor = sp.edit();


        String seekerBG;
        if (sp.getString("bg_donate","").equals("")){
            seekerBG = sp.getString("bg", "");
        }
        else{
            seekerBG = sp.getString("bg_donate", "");
        }

        int selectBG=0;
        for (int i=0; i<groups.length; i++){
            if (seekerBG.equals(groups[i])){
                selectBG=i;
            }
        }
        spinner.setSelection(selectBG);

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener(){
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                editor.putString("bg_donate", parent.getItemAtPosition(position).toString());
                editor.apply();
                donor.setBloodGroup(parent.getItemAtPosition(position).toString());
                Log.d("Selected", parent.getItemAtPosition(position).toString());
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        switchCompat.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) donor.setPrivate(true);
                else donor.setPrivate(false);
            }
        });

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

                usersRecyclerView.setVisibility(View.VISIBLE);
                linearLayout.setVisibility(View.GONE);

                mClient.getTable(BD_Donate.class).insert(donor);

                new FetchTask().execute(mClient);

            }
        });



        return view;
    }

    class FetchTask extends AsyncTask<MobileServiceClient, Void, Boolean>{

        @Override
        protected Boolean doInBackground(MobileServiceClient... params) {

            progressDialog.show();

            MobileServiceTable<BD_Seek> mTable = params[0].getTable("BD_Seek", BD_Seek.class);
            mList = new ArrayList<>();
            try {
                mList = mTable.where().field("bloodGroup").eq(donor.getBloodGroup()).execute().get();
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            }

            if (mList.isEmpty()) {
                Log.d("List", "empty");
            }
            else
                Log.d("Name: ", mList.get(0).getName());

            return true;
        }
        @Override
        protected void onPostExecute(Boolean aBoolean) {
            if (aBoolean) {
                donorResult.clear();
                donorResult.addAll(mList);

                Location locationA = new Location("point A");
                Location locationB = new Location("point B");

                locationA.setLatitude(Double.parseDouble(donor.getLatitude()));
                locationA.setLongitude(Double.parseDouble(donor.getLongitude()));

                for (BD_Seek seeker : mList) {

                    locationB.setLatitude(Double.parseDouble(seeker.getLatitude()));
                    locationB.setLongitude(Double.parseDouble(seeker.getLongitude()));

                    float distance = locationA.distanceTo(locationB);
                    if(distance > 10000)
                        donorResult.remove(seeker);
                }

                if(donorResult.isEmpty())
                {
                    emptyListCondition.setVisibility(View.VISIBLE);
                    fullListCondition.setVisibility(View.GONE);
                    usersRecyclerView.setVisibility(View.GONE);
                }
                else
                {
                    emptyListCondition.setVisibility(View.GONE);
                    fullListCondition.setVisibility(View.VISIBLE);
                    usersRecyclerView.setVisibility(View.VISIBLE);
                }

                donorAdapter.notifyDataSetChanged();

            }

            progressDialog.dismiss();
        }

    }
}
